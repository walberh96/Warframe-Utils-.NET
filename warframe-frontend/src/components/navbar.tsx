"use client";

import { LogOut } from "lucide-react";
import { Button } from "@/components/ui/button";
import { LoginDialog } from "@/components/auth/LoginDialog";
import { RegisterDialog } from "@/components/auth/RegisterDialog";
import { NotificationBell } from "@/components/NotificationBell";
import { useAuth } from "@/contexts/AuthContext";

export function Navbar() {
  const { user, logout } = useAuth();

  return (
    <nav className="border-b border-white/10 backdrop-blur-sm bg-black/20">
      <div className="container mx-auto px-4 py-4 flex items-center justify-between">
        <div className="flex items-center space-x-2">
          <div className="w-8 h-8 rounded-full bg-gradient-to-r from-blue-500 to-purple-600" />
          <span className="text-xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-blue-400 to-purple-400">
            Warframe Utils
          </span>
        </div>

        <div className="flex items-center gap-3">
          {user ? (
            <>
              <NotificationBell />
              <span className="text-sm text-gray-400">{user.email}</span>
              <Button
                variant="outline"
                size="sm"
                onClick={logout}
                className="gap-2"
              >
                <LogOut className="w-4 h-4" />
                Logout
              </Button>
            </>
          ) : (
            <>
              <LoginDialog />
              <RegisterDialog />
            </>
          )}
        </div>
      </div>
    </nav>
  );
}
