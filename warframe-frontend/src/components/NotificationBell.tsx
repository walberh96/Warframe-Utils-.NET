"use client";

import { useState, useEffect } from "react";
import { Bell, Trash2, Edit2, Check, X, AlertCircle, Sparkles } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { useNotifications } from "@/hooks/useNotifications";
import { useToast } from "@/hooks/use-toast";
import { useAuth } from "@/contexts/AuthContext";

interface Alert {
  id: number;
  itemName: string;
  alertPrice: number;
  currentPrice?: number;
  isActive: boolean;
  isTriggered: boolean;
  createdAt: string;
}

export function NotificationBell() {
  const { notifications, unreadCount, markAsRead, markAllAsRead, refreshNotifications } = useNotifications();
  const [alerts, setAlerts] = useState<Alert[]>([]);
  const [editingAlert, setEditingAlert] = useState<Alert | null>(null);
  const [editPrice, setEditPrice] = useState("");
  const [isUpdating, setIsUpdating] = useState(false);
  const [isDeleting, setIsDeleting] = useState<number | null>(null);
  const { toast } = useToast();
  const { user } = useAuth();

  // Fetch alerts and refresh notifications
  useEffect(() => {
    if (user) {
      fetchAlerts();
      refreshNotifications();
      const interval = setInterval(() => {
        fetchAlerts();
        refreshNotifications();
      }, 30000); // Refresh every 30 seconds
      return () => clearInterval(interval);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [user]); // Only depend on user, not refreshNotifications to avoid infinite loops

  const fetchAlerts = async () => {
    try {
      const response = await fetch("/api/Alert", {
        credentials: "include",
      });
      if (response.ok) {
        const data = await response.json();
        setAlerts(data);
      }
    } catch (error) {
      console.error("Failed to fetch alerts:", error);
    }
  };

  const deleteAlert = async (id: number) => {
    setIsDeleting(id);
    try {
      const response = await fetch(`/api/Alert/${id}`, {
        method: "DELETE",
        credentials: "include",
      });

      if (response.ok) {
        toast({
          title: "Success",
          description: "Alert deleted successfully",
        });
        fetchAlerts();
        refreshNotifications();
      } else {
        toast({
          title: "Error",
          description: "Failed to delete alert",
          variant: "destructive",
        });
      }
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete alert",
        variant: "destructive",
      });
    } finally {
      setIsDeleting(null);
    }
  };

  const openEditDialog = (alert: Alert) => {
    setEditingAlert(alert);
    setEditPrice(alert.alertPrice.toString());
  };

  const closeEditDialog = () => {
    setEditingAlert(null);
    setEditPrice("");
  };

  const updateAlert = async () => {
    if (!editingAlert) return;

    const newPrice = parseFloat(editPrice);
    if (isNaN(newPrice) || newPrice < 0) {
      toast({
        title: "Error",
        description: "Please enter a valid price",
        variant: "destructive",
      });
      return;
    }

    setIsUpdating(true);
    try {
      const response = await fetch(`/api/Alert/${editingAlert.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({
          alertPrice: newPrice,
        }),
      });

      if (response.ok) {
        toast({
          title: "Success",
          description: "Alert updated successfully",
        });
        closeEditDialog();
        fetchAlerts();
        refreshNotifications();
      } else {
        toast({
          title: "Error",
          description: "Failed to update alert",
          variant: "destructive",
        });
      }
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to update alert",
        variant: "destructive",
      });
    } finally {
      setIsUpdating(false);
    }
  };

  const totalAlerts = alerts.length;
  const triggeredAlerts = alerts.filter(a => a.isTriggered).length;
  const badgeCount = unreadCount + (triggeredAlerts > 0 ? 1 : 0);

  return (
    <>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="ghost" size="icon" className="relative">
            <Bell className="h-5 w-5" />
            {(badgeCount > 0 || totalAlerts > 0) && (
              <Badge
                variant="destructive"
                className="absolute -top-1 -right-1 h-5 w-5 flex items-center justify-center p-0 text-xs"
              >
                {badgeCount > 9 ? "9+" : badgeCount || totalAlerts}
              </Badge>
            )}
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" className="w-96 p-0">
          <Tabs defaultValue="alerts" className="w-full">
            <TabsList className="grid w-full grid-cols-2 m-2">
              <TabsTrigger value="alerts" className="text-xs">
                <Sparkles className="w-3 h-3 mr-1" />
                Alerts ({totalAlerts})
              </TabsTrigger>
              <TabsTrigger value="notifications" className="text-xs">
                <AlertCircle className="w-3 h-3 mr-1" />
                Notifications ({unreadCount})
              </TabsTrigger>
            </TabsList>

            {/* Alerts Tab */}
            <TabsContent value="alerts" className="mt-0 p-2">
              <div className="max-h-96 overflow-y-auto space-y-2">
                {alerts.length === 0 ? (
                  <div className="p-6 text-center">
                    <Bell className="w-12 h-12 mx-auto mb-3 opacity-20 text-gray-500" />
                    <p className="text-sm text-gray-400 mb-1">No alerts yet</p>
                    <p className="text-xs text-gray-500">Search for items to create price alerts</p>
                  </div>
                ) : (
                  alerts.map((alert) => (
                    <div
                      key={alert.id}
                      className={`p-3 rounded-lg border transition-all ${
                        alert.isTriggered
                          ? "bg-gradient-to-r from-red-950/40 to-orange-950/40 border-red-500/50 animate-pulse"
                          : "bg-slate-900/50 border-purple-500/20 hover:border-purple-500/40"
                      }`}
                    >
                      <div className="flex items-start justify-between gap-2">
                        <div className="flex-1 min-w-0">
                          <div className="flex items-center gap-2 mb-1">
                            <span className="font-semibold text-sm text-gray-200 truncate">
                              {alert.itemName}
                            </span>
                            {alert.isTriggered && (
                              <Badge variant="destructive" className="text-xs px-1.5 py-0">
                                Triggered
                              </Badge>
                            )}
                          </div>
                          <div className="text-xs text-gray-400 space-y-0.5">
                            <div>
                              Target: <span className="text-purple-400 font-semibold">{alert.alertPrice}p</span>
                            </div>
                            {alert.currentPrice !== undefined && (
                              <div>
                                Current: <span className="text-blue-400 font-semibold">{alert.currentPrice}p</span>
                              </div>
                            )}
                            <div className="text-gray-500">
                              {new Date(alert.createdAt).toLocaleDateString()}
                            </div>
                          </div>
                        </div>
                        <div className="flex items-center gap-1 shrink-0">
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-7 w-7 text-blue-400 hover:text-blue-300 hover:bg-blue-500/10"
                            onClick={() => openEditDialog(alert)}
                            title="Modify alert"
                          >
                            <Edit2 className="w-3.5 h-3.5" />
                          </Button>
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-7 w-7 text-red-400 hover:text-red-300 hover:bg-red-500/10"
                            onClick={() => deleteAlert(alert.id)}
                            disabled={isDeleting === alert.id}
                            title="Delete alert"
                          >
                            {isDeleting === alert.id ? (
                              <div className="w-3.5 h-3.5 border-2 border-red-400 border-t-transparent rounded-full animate-spin" />
                            ) : (
                              <Trash2 className="w-3.5 h-3.5" />
                            )}
                          </Button>
                        </div>
                      </div>
                    </div>
                  ))
                )}
              </div>
            </TabsContent>

            {/* Notifications Tab */}
            <TabsContent value="notifications" className="mt-0 p-2">
              <div className="flex items-center justify-between mb-2 px-2">
                <span className="text-xs text-gray-400">Price Alert Notifications</span>
                {unreadCount > 0 && (
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={markAllAsRead}
                    className="text-xs h-6 px-2"
                  >
                    Mark all read
                  </Button>
                )}
              </div>
              <div className="max-h-96 overflow-y-auto space-y-2">
                {notifications.length === 0 ? (
                  <div className="p-6 text-center">
                    <AlertCircle className="w-12 h-12 mx-auto mb-3 opacity-20 text-gray-500" />
                    <p className="text-sm text-gray-400">No new notifications</p>
                  </div>
                ) : (
                  notifications.map((notification) => (
                    <div
                      key={notification.id}
                      className={`p-3 rounded-lg border transition-all cursor-pointer ${
                        notification.isRead 
                          ? "bg-slate-800/40 border-gray-500/20 opacity-60" 
                          : "bg-gradient-to-r from-green-950/40 to-emerald-950/40 border-green-500/30 hover:border-green-500/50"
                      }`}
                      onClick={() => !notification.isRead && markAsRead(notification.id)}
                    >
                      <div className="flex items-start gap-2">
                        <AlertCircle className={`w-4 h-4 mt-0.5 shrink-0 ${notification.isRead ? "text-gray-500" : "text-green-400"}`} />
                        <div className="flex-1 min-w-0">
                          <div className={`font-semibold text-sm mb-1 ${notification.isRead ? "text-gray-400" : "text-gray-200"}`}>
                            🎯 {notification.itemName}
                          </div>
                          <div className={`text-xs ${notification.isRead ? "text-gray-500" : "text-gray-300"}`}>
                            Price dropped to{" "}
                            <span className={`font-bold ${notification.isRead ? "text-gray-400" : "text-green-400"}`}>{notification.triggeredPrice}p</span>
                          </div>
                          <div className="text-xs text-gray-500 mt-1">
                            {new Date(notification.createdAt).toLocaleString()}
                            {notification.isRead && " • Read"}
                          </div>
                        </div>
                        {!notification.isRead && (
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-6 w-6 shrink-0"
                            onClick={(e) => {
                              e.stopPropagation();
                              markAsRead(notification.id);
                            }}
                          >
                            <Check className="w-3.5 h-3.5" />
                          </Button>
                        )}
                      </div>
                    </div>
                  ))
                )}
              </div>
            </TabsContent>
          </Tabs>
        </DropdownMenuContent>
      </DropdownMenu>

      {/* Edit Alert Dialog */}
      <Dialog open={editingAlert !== null} onOpenChange={closeEditDialog}>
        <DialogContent className="sm:max-w-md">
          <DialogHeader>
            <DialogTitle>Modify Price Alert</DialogTitle>
            <DialogDescription>
              Update the target price for {editingAlert?.itemName}
            </DialogDescription>
          </DialogHeader>
          <div className="space-y-4 py-4">
            <div>
              <label className="text-sm font-medium text-gray-300 mb-2 block">
                Current Target Price:{" "}
                <span className="text-purple-400 font-bold">{editingAlert?.alertPrice}p</span>
              </label>
              <Input
                type="number"
                placeholder="Enter new target price"
                value={editPrice}
                onChange={(e) => setEditPrice(e.target.value)}
                min="0"
                step="0.01"
                className="mt-2"
                autoFocus
              />
            </div>
          </div>
          <DialogFooter>
            <Button
              variant="outline"
              onClick={closeEditDialog}
              disabled={isUpdating}
            >
              <X className="w-4 h-4 mr-2" />
              Cancel
            </Button>
            <Button
              onClick={updateAlert}
              disabled={isUpdating}
              className="bg-purple-600 hover:bg-purple-700"
            >
              <Check className="w-4 h-4 mr-2" />
              {isUpdating ? "Updating..." : "Update Alert"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </>
  );
}
