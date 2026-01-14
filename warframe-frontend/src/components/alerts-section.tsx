"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Bell, Trash2, AlertCircle, Lock, Edit2, X, Check } from "lucide-react";
import { useToast } from "@/hooks/use-toast";
import { useAuth } from "@/contexts/AuthContext";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";

interface Alert {
  id: number;
  itemName: string;
  alertPrice: number;
  currentPrice?: number;
  isActive: boolean;
  isTriggered: boolean;
  createdAt: string;
}

export function AlertsSection() {
  const [alerts, setAlerts] = useState<Alert[]>([]);
  const [editingAlert, setEditingAlert] = useState<Alert | null>(null);
  const [editPrice, setEditPrice] = useState("");
  const [isUpdating, setIsUpdating] = useState(false);
  const { toast } = useToast();
  const { user } = useAuth();

  useEffect(() => {
    if (user) {
      fetchAlerts();
    }
  }, [user]);

  const fetchAlerts = async () => {
    try {
      const response = await fetch("http://localhost:5089/api/Alert", {
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

  // Refresh alerts every 30 seconds to check for triggered alerts
  useEffect(() => {
    if (user) {
      const interval = setInterval(() => {
        fetchAlerts();
      }, 30000);
      return () => clearInterval(interval);
    }
  }, [user]);

  const deleteAlert = async (id: number) => {
    try {
      const response = await fetch(`http://localhost:5089/api/Alert/${id}`, {
        method: "DELETE",
        credentials: "include",
      });

      if (response.ok) {
        toast({
          title: "Success",
          description: "Alert deleted successfully",
        });
        fetchAlerts();
      }
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete alert",
        variant: "destructive",
      });
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
      const response = await fetch(`http://localhost:5089/api/Alert/${editingAlert.id}`, {
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

  if (!user) {
    return (
      <Card className="warframe-card border-purple-500/20">
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Bell className="w-6 h-6 text-purple-400" />
            Price Alerts
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-center text-gray-400 py-12">
            <Lock className="w-16 h-16 mx-auto mb-4 opacity-20" />
            <p className="text-lg mb-2">Login Required</p>
            <p className="text-sm">Please login to view and create price alerts</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card id="alerts-section" className="warframe-card border-purple-500/20">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Bell className="w-6 h-6 text-purple-400" />
          Price Alerts
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">
        {/* Triggered Alerts - Show RED ALERT */}
        {alerts.some(a => a.isTriggered) && (
          <div className="space-y-3">
            <h3 className="text-lg font-semibold text-red-400">🔴 Price Alerts Triggered!</h3>
            {alerts
              .filter(a => a.isTriggered)
              .map((alert) => (
                <div
                  key={alert.id}
                  className="flex justify-between items-center p-4 rounded-lg bg-red-950/40 border-2 border-red-500/80 animate-pulse"
                >
                  <div className="flex-1">
                    <div className="font-bold text-red-300 text-lg">{alert.itemName}</div>
                    <div className="text-sm text-red-200 mt-2">
                      <span className="font-bold">✓ PRICE DROP DETECTED!</span>
                    </div>
                    <div className="text-sm text-red-200 mt-1">
                      Target: <span className="text-red-400 font-bold">{alert.alertPrice}p</span>
                    </div>
                    <div className="text-xs text-red-300 mt-1">
                      Created: {new Date(alert.createdAt).toLocaleDateString()}
                    </div>
                  </div>
                  <div className="flex items-center gap-2">
                    <AlertCircle className="w-6 h-6 text-red-500 animate-bounce" />
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => deleteAlert(alert.id)}
                      className="text-red-400 hover:text-red-300"
                    >
                      <Trash2 className="w-4 h-4" />
                    </Button>
                  </div>
                </div>
              ))}
          </div>
        )}

        {/* Active Alerts List */}
        <div className="space-y-3">
          <h3 className="text-lg font-semibold text-purple-300">Your Alerts</h3>
          {alerts.length > 0 ? (
            alerts
              .filter(a => !a.isTriggered)
              .map((alert) => (
                <div
                  key={alert.id}
                  className="flex justify-between items-center p-4 rounded-lg bg-card/50 border border-purple-500/20 hover:border-purple-500/40 transition-all"
                >
                  <div className="flex-1">
                    <div className="font-medium">{alert.itemName}</div>
                    <div className="text-sm text-gray-400">
                      Target Price: <span className="text-purple-400 font-bold">{alert.alertPrice}p</span>
                    </div>
                    <div className="text-xs text-gray-500 mt-1">
                      Created: {new Date(alert.createdAt).toLocaleDateString()}
                    </div>
                  </div>
                  <div className="flex items-center gap-2">
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => openEditDialog(alert)}
                      className="text-blue-400 hover:text-blue-300"
                      title="Modify alert"
                    >
                      <Edit2 className="w-4 h-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => deleteAlert(alert.id)}
                      className="text-red-400 hover:text-red-300"
                      title="Delete alert"
                    >
                      <Trash2 className="w-4 h-4" />
                    </Button>
                  </div>
                </div>
              ))
          ) : (
            <div className="text-center text-gray-500 py-8">
              <Bell className="w-16 h-16 mx-auto mb-4 opacity-20" />
              <p>No active alerts. Search for an item to create one!</p>
            </div>
          )}
        </div>
      </CardContent>

      {/* Edit Alert Dialog */}
      <Dialog open={editingAlert !== null} onOpenChange={closeEditDialog}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Modify Price Alert</DialogTitle>
            <DialogDescription>
              Update the target price for {editingAlert?.itemName}
            </DialogDescription>
          </DialogHeader>
          <div className="space-y-4 py-4">
            <div>
              <label className="text-sm font-medium text-gray-300 mb-2 block">
                Current Target Price: <span className="text-purple-400 font-bold">{editingAlert?.alertPrice}p</span>
              </label>
              <Input
                type="number"
                placeholder="Enter new target price"
                value={editPrice}
                onChange={(e) => setEditPrice(e.target.value)}
                min="0"
                step="0.01"
                className="mt-2"
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
    </Card>
  );
}
