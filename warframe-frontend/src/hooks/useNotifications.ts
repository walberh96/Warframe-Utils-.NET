"use client";

import { useEffect, useState, useRef } from "react";
import { useToast } from "@/hooks/use-toast";
import { useAuth } from "@/contexts/AuthContext";

interface Notification {
  id: number;
  message: string;
  createdAt: string;
  isRead: boolean;
  triggeredPrice: number;
  itemName: string;
  priceAlertId: number;
}

export function useNotifications() {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const { toast } = useToast();
  const { user } = useAuth();
  const toastRef = useRef(toast);
  
  // Keep toast ref updated
  useEffect(() => {
    toastRef.current = toast;
  }, [toast]);

  useEffect(() => {
    if (!user) return;

    const fetchNotifications = async () => {
      try {
        const response = await fetch("/api/Alert/notifications/unread", {
          credentials: "include",
        });

        if (response.ok) {
          const data = await response.json();
          const newNotifications = data as Notification[];
          
          console.log("Fetched notifications:", newNotifications);
          
          // Use functional update to get current state
          setNotifications((prevNotifications) => {
            // Check if there are new unread notifications
            const previousIds = new Set(prevNotifications.map(n => n.id));
            const newOnes = newNotifications.filter(n => !previousIds.has(n.id));
            
            // Show popup toast for each new notification (only once)
            if (newOnes.length > 0) {
              console.log("New notifications found:", newOnes);
              newOnes.forEach((notification) => {
                toastRef.current({
                  title: "🎯 Price Alert Triggered!",
                  description: `${notification.itemName} is now at ${notification.triggeredPrice} platinum`,
                  duration: 10000,
                  className: "bg-gradient-to-r from-red-900/90 to-orange-900/90 border-red-500/50",
                });
              });
            }

            return newNotifications;
          });
          
          setUnreadCount(newNotifications.length);
        } else {
          console.error("Failed to fetch notifications - status:", response.status, response.statusText);
        }
      } catch (error) {
        console.error("Error fetching notifications:", error);
      }
    };

    // Initial fetch immediately
    fetchNotifications();

    // Poll for new notifications every 30 seconds
    const interval = setInterval(() => {
      fetchNotifications();
    }, 30000);

    return () => clearInterval(interval);
  }, [user]);

  const markAsRead = async (notificationId: number) => {
    try {
      const response = await fetch(
        `/api/Alert/notifications/${notificationId}/read`,
        {
          method: "POST",
          credentials: "include",
        }
      );

      if (response.ok) {
        // Update local state - mark as read but keep in list
        setNotifications((prev) => 
          prev.map(n => n.id === notificationId ? { ...n, isRead: true } : n)
        );
        setUnreadCount((prev) => Math.max(0, prev - 1));
      }
    } catch (error) {
      console.error("Failed to mark notification as read:", error);
    }
  };

  const markAllAsRead = async () => {
    try {
      await Promise.all(notifications.filter(n => !n.isRead).map((n) => markAsRead(n.id)));
      // Mark all as read in local state but keep them visible
      setNotifications((prev) => prev.map(n => ({ ...n, isRead: true })));
      setUnreadCount(0);
    } catch (error) {
      console.error("Failed to mark all as read:", error);
    }
  };

  const refreshNotifications = async () => {
    if (!user) return;

    try {
      const response = await fetch("/api/Alert/notifications/unread", {
        credentials: "include",
      });

      if (response.ok) {
        const data = await response.json();
        const newNotifications = data as Notification[];
        
        console.log("Refreshed notifications:", newNotifications);
        
        setNotifications((prevNotifications) => {
          const previousIds = new Set(prevNotifications.map(n => n.id));
          const newOnes = newNotifications.filter(n => !previousIds.has(n.id));
          
          if (newOnes.length > 0) {
            console.log("New notifications on refresh:", newOnes);
            newOnes.forEach((notification) => {
              toastRef.current({
                title: "🎯 Price Alert Triggered!",
                description: `${notification.itemName} is now at ${notification.triggeredPrice} platinum`,
                duration: 10000,
                className: "bg-gradient-to-r from-red-900/90 to-orange-900/90 border-red-500/50",
              });
            });
          }

          return newNotifications;
        });
        
        setUnreadCount(newNotifications.length);
      } else {
        console.error("Failed to refresh notifications - status:", response.status);
      }
    } catch (error) {
      console.error("Error refreshing notifications:", error);
    }
  };

  return {
    notifications,
    unreadCount,
    markAsRead,
    markAllAsRead,
    refreshNotifications,
  };
}
