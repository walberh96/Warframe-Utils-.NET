"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Clock, Users, Snowflake } from "lucide-react";

interface GameStatus {
  cetusCycle?: {
    state: string;
    timeLeft: string;
  };
  voidTrader?: {
    active: boolean;
    character: string;
    location?: string;
    startString?: string;
    endString?: string;
  };
  vallisCycle?: {
    state: string;
    timeLeft: string;
  };
}

export function GameStatusSection() {
  const [status, setStatus] = useState<GameStatus>({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchGameStatus();
    const interval = setInterval(fetchGameStatus, 60000); // Update every minute
    return () => clearInterval(interval);
  }, []);

  const fetchGameStatus = async () => {
    try {
      // This will proxy to the .NET backend
      const response = await fetch("/api/gameStatus");
      if (response.ok) {
        const data = await response.json();
        setStatus(data);
      }
    } catch (error) {
      console.error("Failed to fetch game status:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
      {/* Cetus Cycle */}
      <Card className="warframe-card border-blue-500/20 hover:border-blue-500/40 transition-all">
        <CardHeader>
          <CardTitle className="flex items-center gap-2 text-lg">
            <Clock className="w-5 h-5 text-blue-400" />
            Cetus Cycle
          </CardTitle>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="animate-pulse">
              <div className="h-4 bg-gray-700 rounded mb-2"></div>
              <div className="h-3 bg-gray-700 rounded w-2/3"></div>
            </div>
          ) : status.cetusCycle ? (
            <>
              <div className="text-2xl font-bold text-blue-400">
                {status.cetusCycle.state}
              </div>
              <div className="text-sm text-gray-400">
                {status.cetusCycle.timeLeft}
              </div>
            </>
          ) : (
            <div className="text-sm text-gray-500">No data</div>
          )}
        </CardContent>
      </Card>

      {/* Void Trader */}
      <Card className="warframe-card border-purple-500/20 hover:border-purple-500/40 transition-all">
        <CardHeader>
          <CardTitle className="flex items-center gap-2 text-lg">
            <Users className="w-5 h-5 text-purple-400" />
            Void Trader
          </CardTitle>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="animate-pulse">
              <div className="h-4 bg-gray-700 rounded mb-2"></div>
              <div className="h-3 bg-gray-700 rounded w-2/3"></div>
            </div>
          ) : status.voidTrader ? (
            <>
              <div className="text-2xl font-bold text-purple-400">
                {status.voidTrader.active ? "Active" : "Away"}
              </div>
              <div className="text-sm text-gray-400">
                {status.voidTrader.location || "Not visiting"}
              </div>
            </>
          ) : (
            <div className="text-sm text-gray-500">No data</div>
          )}
        </CardContent>
      </Card>

      {/* Venus Cycle (Orb Vallis) */}
      <Card className="warframe-card border-pink-500/20 hover:border-pink-500/40 transition-all">
        <CardHeader>
          <CardTitle className="flex items-center gap-2 text-lg">
            <Snowflake className="w-5 h-5 text-pink-400" />
            Venus Cycle
          </CardTitle>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="animate-pulse">
              <div className="h-4 bg-gray-700 rounded mb-2"></div>
              <div className="h-3 bg-gray-700 rounded w-2/3"></div>
            </div>
          ) : status.vallisCycle ? (
            <>
              <div className="text-2xl font-bold text-pink-400">
                {status.vallisCycle.state}
              </div>
              <div className="text-sm text-gray-400">
                {status.vallisCycle.timeLeft}
              </div>
            </>
          ) : (
            <div className="text-sm text-gray-500">No data</div>
          )}
        </CardContent>
      </Card>

    </div>
  );
}
