"use client";

import { useState, useEffect, useRef } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Search, TrendingUp, TrendingDown, Copy, Check, ExternalLink, Bell, Plus } from "lucide-react";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useToast } from "@/hooks/use-toast";
import { useAuth } from "@/contexts/AuthContext";
import { useNotifications } from "@/hooks/useNotifications";

interface ModItem {
  id: string;
  url_name: string;
  item_name: string;
  thumb?: string;
  wiki_link?: string;
}

interface ModDetails {
  item_name: string;
  description?: string;
  thumb?: string;
  icon?: string;
  rarity?: string;
  trading_tax?: number;
  wiki_link?: string;
  url_name?: string;
}

interface Order {
  order_type: string;
  platinum: number;
  quantity: number;
  user: {
    ingame_name: string;
    status: string;
  };
}

interface AutocompleteItem {
  url_name: string;
  item_name: string;
}

export function SearchSection() {
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedMod, setSelectedMod] = useState<string | null>(null);
  const [modDetails, setModDetails] = useState<ModDetails | null>(null);
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(false);
  const [copiedUser, setCopiedUser] = useState<string | null>(null);
  const [alertPrice, setAlertPrice] = useState("");
  const [creatingAlert, setCreatingAlert] = useState(false);
  const [autocompleteItems, setAutocompleteItems] = useState<AutocompleteItem[]>([]);
  const [showSuggestions, setShowSuggestions] = useState(false);
  const [filteredSuggestions, setFilteredSuggestions] = useState<AutocompleteItem[]>([]);
  const searchInputRef = useRef<HTMLInputElement>(null);
  const suggestionsRef = useRef<HTMLDivElement>(null);
  const { toast } = useToast();
  const { user } = useAuth();
  const { refreshNotifications } = useNotifications();

  // Load autocomplete items on mount
  useEffect(() => {
    const loadAutocompleteItems = async () => {
      try {
        const response = await fetch("/api/search/items");
        if (response.ok) {
          const data = await response.json();
          setAutocompleteItems(data);
        }
      } catch (error) {
        console.error("Failed to load autocomplete items:", error);
      }
    };
    loadAutocompleteItems();
  }, []);

  // Filter suggestions based on search query
  useEffect(() => {
    if (searchQuery.trim().length > 0) {
      const filtered = autocompleteItems
        .filter(item => 
          item.item_name.toLowerCase().includes(searchQuery.toLowerCase())
        )
        .slice(0, 10); // Limit to 10 suggestions
      setFilteredSuggestions(filtered);
      setShowSuggestions(filtered.length > 0);
    } else {
      setShowSuggestions(false);
    }
  }, [searchQuery, autocompleteItems]);

  // Close suggestions when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        suggestionsRef.current &&
        !suggestionsRef.current.contains(event.target as Node) &&
        searchInputRef.current &&
        !searchInputRef.current.contains(event.target as Node)
      ) {
        setShowSuggestions(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const selectSuggestion = (item: AutocompleteItem) => {
    const itemName = item.item_name;
    setSearchQuery(itemName);
    setShowSuggestions(false);
    // Auto-search immediately with the selected item name
    performSearch(itemName);
  };

  const performSearch = async (queryToSearch: string) => {
    if (!queryToSearch.trim()) {
      toast({
        title: "Search Required",
        description: "Please enter an item name to search",
        variant: "destructive",
      });
      return;
    }
    
    setLoading(true);
    try {
      const response = await fetch(`/api/search?modName=${encodeURIComponent(queryToSearch.trim())}`);
      const data = await response.json();
      
      if (response.ok) {
        if (data.modDetails) {
          setModDetails(data.modDetails);
          setOrders(data.orders || []);
          setSelectedMod(data.modDetails.item_name || queryToSearch);
          // Set default alert price to lowest sell order
          const sellOrders = (data.orders || []).filter((o: Order) => o.order_type === "sell");
          if (sellOrders.length > 0) {
            setAlertPrice(sellOrders[0].platinum.toString());
          } else {
            setAlertPrice("");
          }
          
          // Check for new notifications when searching (user might be checking prices)
          if (user) {
            refreshNotifications();
          }
        } else {
          toast({
            title: "Item Not Found",
            description: `Could not find "${queryToSearch}" in the market`,
            variant: "destructive",
          });
          setModDetails(null);
          setOrders([]);
          setSelectedMod(null);
        }
      } else {
        const errorMessage = data.error || "Failed to search for item";
        toast({
          title: "Error",
          description: errorMessage,
          variant: "destructive",
        });
        setModDetails(null);
        setOrders([]);
        setSelectedMod(null);
      }
    } catch (error) {
      console.error("Search failed:", error);
      toast({
        title: "Error",
        description: "Network error. Please check your connection and try again.",
        variant: "destructive",
      });
      setModDetails(null);
      setOrders([]);
      setSelectedMod(null);
    } finally {
      setLoading(false);
    }
  };

  const getImageUrl = (modDetails: ModDetails | null) => {
    if (!modDetails) return null;
    // Prefer icon over thumb for better quality, fallback to thumb
    const imagePath = modDetails.icon || modDetails.thumb;
    if (!imagePath) return null;
    // Handle both full URLs and relative paths
    if (imagePath.startsWith('http')) return imagePath;
    return `https://warframe.market/static/assets/${imagePath}`;
  };

  const handleSearch = async () => {
    performSearch(searchQuery);
  };

  const copyUsername = async (username: string) => {
    try {
      await navigator.clipboard.writeText(username);
      setCopiedUser(username);
      toast({
        title: "Copied!",
        description: `${username} copied to clipboard`,
      });
      setTimeout(() => setCopiedUser(null), 2000);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to copy username",
        variant: "destructive",
      });
    }
  };

  const createAlert = async () => {
    if (!user) {
      toast({
        title: "Login Required",
        description: "Please login to create price alerts",
        variant: "destructive",
      });
      return;
    }

    if (!alertPrice || parseFloat(alertPrice) <= 0) {
      toast({
        title: "Error",
        description: "Please enter a valid alert price",
        variant: "destructive",
      });
      return;
    }

    setCreatingAlert(true);
    try {
      const response = await fetch("/api/Alert", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({
          itemName: selectedMod,
          itemId: modDetails?.url_name, // Pass the URL name for more reliable price checking
          alertPrice: parseFloat(alertPrice),
        }),
      });

      if (response.ok) {
        toast({
          title: "Success!",
          description: `Price alert created for ${selectedMod} at ${alertPrice}p`,
        });
        setAlertPrice("");
        // Wait a moment for backend to process, then check for notifications
        setTimeout(() => {
          refreshNotifications();
        }, 1000);
      } else {
        toast({
          title: "Error",
          description: "Failed to create alert",
          variant: "destructive",
        });
      }
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to create alert",
        variant: "destructive",
      });
    } finally {
      setCreatingAlert(false);
    }
  };

  // Sort orders: online first, then by price
  const sortOrders = (ordersList: Order[], type: "sell" | "buy") => {
    return [...ordersList]
      .filter(o => o.order_type === type)
      .sort((a, b) => {
        // Online users first
        if (a.user.status === "ingame" && b.user.status !== "ingame") return -1;
        if (a.user.status !== "ingame" && b.user.status === "ingame") return 1;
        // Then by price
        return type === "sell" ? a.platinum - b.platinum : b.platinum - a.platinum;
      });
  };

  const sellOrders = sortOrders(orders, "sell");
  const buyOrders = sortOrders(orders, "buy");

  const getWikiLink = () => {
    if (modDetails?.wiki_link) return modDetails.wiki_link;
    if (selectedMod) {
      return `https://warframe.fandom.com/wiki/${encodeURIComponent(selectedMod.replace(/ /g, "_"))}`;
    }
    return null;
  };

  return (
    <Card className="warframe-card border-blue-500/20">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Search className="w-6 h-6 text-blue-400" />
          Market Search
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">
        {/* Search Bar */}
        <div className="flex gap-2">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400 z-10" />
            <Input
              ref={searchInputRef}
              type="text"
              placeholder="Search for mods, items, or sets (e.g., Serration, Energy Nexus)..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter" && !loading) {
                  if (showSuggestions && filteredSuggestions.length > 0) {
                    selectSuggestion(filteredSuggestions[0]);
                  } else {
                    handleSearch();
                  }
                } else if (e.key === "Escape") {
                  setShowSuggestions(false);
                }
              }}
              onFocus={() => {
                if (filteredSuggestions.length > 0) {
                  setShowSuggestions(true);
                }
              }}
              className="pl-10 pr-4 h-11"
              disabled={loading}
            />
            {/* Autocomplete Suggestions */}
            {showSuggestions && filteredSuggestions.length > 0 && (
              <div
                ref={suggestionsRef}
                className="absolute z-50 w-full mt-1 bg-slate-900 border border-blue-500/30 rounded-lg shadow-lg max-h-60 overflow-y-auto"
              >
                {filteredSuggestions.map((item, index) => (
                  <div
                    key={`${item.url_name}-${index}`}
                    onClick={() => selectSuggestion(item)}
                    className="px-4 py-2 hover:bg-blue-900/30 cursor-pointer transition-colors border-b border-blue-500/10 last:border-b-0"
                  >
                    <div className="text-sm text-gray-200 font-medium">{item.item_name}</div>
                  </div>
                ))}
              </div>
            )}
          </div>
          <Button 
            onClick={handleSearch} 
            disabled={loading || !searchQuery.trim()}
            className="h-11 px-6"
          >
            {loading ? (
              <>
                <span className="animate-spin mr-2">⏳</span>
                Searching...
              </>
            ) : (
              <>
                <Search className="w-4 h-4 mr-2" />
                Search
              </>
            )}
          </Button>
        </div>

        {/* Results */}
        {selectedMod && modDetails && (
          <div className="space-y-4">
            {/* Mod Details with Large Image */}
            <div className="p-6 rounded-lg bg-gradient-to-b from-blue-900/40 to-purple-900/30 border-2 border-blue-500/40 backdrop-blur-sm">
              <div className="flex flex-col md:flex-row gap-6 items-start">
                {getImageUrl(modDetails) && (
                  <div className="flex-shrink-0 mx-auto md:mx-0">
                    <div className="relative group">
                      <img 
                        src={getImageUrl(modDetails)!}
                        alt={modDetails.item_name || selectedMod}
                        className="w-48 h-48 md:w-56 md:h-56 rounded-xl border-2 border-blue-400/50 object-contain bg-gradient-to-br from-blue-950/50 to-purple-950/50 p-2 shadow-lg shadow-blue-500/20 group-hover:shadow-blue-500/40 transition-all group-hover:scale-105"
                        onError={(e) => {
                          // Fallback if image fails to load
                          const target = e.target as HTMLImageElement;
                          target.style.display = 'none';
                        }}
                      />
                      <div className="absolute inset-0 rounded-xl bg-gradient-to-t from-blue-900/40 to-transparent opacity-0 group-hover:opacity-100 transition-opacity pointer-events-none" />
                    </div>
                  </div>
                )}
                <div className="flex-1 w-full">
                  <div className="flex flex-col md:flex-row md:items-start md:justify-between gap-4">
                    <div className="space-y-4 flex-1">
                      <div>
                        <h2 className="text-3xl md:text-4xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-blue-300 via-purple-300 to-pink-300 mb-2">
                          {modDetails.item_name || selectedMod}
                        </h2>
                        {modDetails.description && (
                          <p className="text-gray-300 text-sm md:text-base leading-relaxed mt-3 p-4 rounded-lg bg-black/20 border border-blue-500/20">
                            {modDetails.description}
                          </p>
                        )}
                      </div>
                      <div className="flex flex-wrap gap-4 text-sm">
                        {modDetails.rarity && (
                          <div className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-blue-900/30 border border-blue-500/30">
                            <span className="text-blue-300 font-semibold">Rarity:</span>
                            <span className="text-gray-200 capitalize">{modDetails.rarity}</span>
                          </div>
                        )}
                        {modDetails.trading_tax !== undefined && modDetails.trading_tax > 0 && (
                          <div className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-purple-900/30 border border-purple-500/30">
                            <span className="text-purple-300 font-semibold">Trading Tax:</span>
                            <span className="text-gray-200">{modDetails.trading_tax.toLocaleString()} credits</span>
                          </div>
                        )}
                      </div>
                    </div>
                    {getWikiLink() && (
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => window.open(getWikiLink()!, "_blank")}
                        className="gap-2 border-blue-400/50 hover:border-blue-400 hover:bg-blue-500/10 shrink-0"
                      >
                        <ExternalLink className="w-4 h-4" />
                        Wiki
                      </Button>
                    )}
                  </div>
                </div>
              </div>
            </div>

            {/* Create Alert */}
            {user && (
              <div className="p-4 rounded-lg bg-gradient-to-r from-purple-900/20 to-pink-900/20 border border-purple-500/30">
                <div className="flex items-center gap-3">
                  <Bell className="w-5 h-5 text-purple-400" />
                  <Input
                    type="number"
                    placeholder="Alert price (platinum)"
                    value={alertPrice}
                    onChange={(e) => setAlertPrice(e.target.value)}
                    className="flex-1"
                  />
                  <Button
                    onClick={createAlert}
                    disabled={creatingAlert}
                    className="bg-purple-600 hover:bg-purple-700 gap-2"
                  >
                    <Plus className="w-4 h-4" />
                    {creatingAlert ? "Creating..." : "Create Alert"}
                  </Button>
                </div>
              </div>
            )}

            {/* Orders Tabs */}
            <Tabs defaultValue="sell" className="w-full">
              <TabsList className="grid w-full grid-cols-2">
                <TabsTrigger value="sell" className="flex items-center gap-2">
                  <TrendingUp className="w-4 h-4" />
                  Sell Orders ({sellOrders.length})
                </TabsTrigger>
                <TabsTrigger value="buy" className="flex items-center gap-2">
                  <TrendingDown className="w-4 h-4" />
                  Buy Orders ({buyOrders.length})
                </TabsTrigger>
              </TabsList>

              <TabsContent value="sell" className="space-y-2 mt-4">
                {sellOrders.length > 0 ? (
                  sellOrders.slice(0, 15).map((order, idx) => (
                    <div
                      key={idx}
                      className={`flex justify-between items-center p-4 rounded-lg backdrop-blur-sm transition-all border-2 ${
                        order.user.status === "ingame" 
                          ? "bg-green-900/30 border-green-500/60 hover:bg-green-900/50 shadow-lg shadow-green-500/10" 
                          : "bg-green-900/10 border-green-500/20 hover:bg-green-900/25"
                      }`}
                    >
                      <div className="flex-1">
                        <div className="flex items-center gap-3">
                          <div className={`w-3 h-3 rounded-full ${order.user.status === "ingame" ? "bg-green-400" : "bg-gray-500"}`} />
                          <span className="font-semibold text-lg">{order.user.ingame_name}</span>
                          {order.user.status === "ingame" && (
                            <span className="text-xs bg-green-500/30 text-green-300 px-2 py-1 rounded-full">Online</span>
                          )}
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => copyUsername(order.user.ingame_name)}
                            className="h-6 w-6 p-0 ml-auto mr-3"
                          >
                            {copiedUser === order.user.ingame_name ? (
                              <Check className="w-4 h-4 text-green-400" />
                            ) : (
                              <Copy className="w-4 h-4 text-gray-400 hover:text-gray-200" />
                            )}
                          </Button>
                        </div>
                      </div>
                      <div className="text-right">
                        <div className="text-2xl font-bold text-green-400">{order.platinum}p</div>
                        <div className="text-xs text-gray-400">Qty: {order.quantity}</div>
                      </div>
                    </div>
                  ))
                ) : (
                  <div className="text-center py-8 text-gray-500">No sell orders available</div>
                )}
              </TabsContent>

              <TabsContent value="buy" className="space-y-2 mt-4">
                {buyOrders.length > 0 ? (
                  buyOrders.slice(0, 15).map((order, idx) => (
                    <div
                      key={idx}
                      className={`flex justify-between items-center p-4 rounded-lg backdrop-blur-sm transition-all border-2 ${
                        order.user.status === "ingame" 
                          ? "bg-orange-900/30 border-orange-500/60 hover:bg-orange-900/50 shadow-lg shadow-orange-500/10" 
                          : "bg-orange-900/10 border-orange-500/20 hover:bg-orange-900/25"
                      }`}
                    >
                      <div className="flex-1">
                        <div className="flex items-center gap-3">
                          <div className={`w-3 h-3 rounded-full ${order.user.status === "ingame" ? "bg-orange-400" : "bg-gray-500"}`} />
                          <span className="font-semibold text-lg">{order.user.ingame_name}</span>
                          {order.user.status === "ingame" && (
                            <span className="text-xs bg-orange-500/30 text-orange-300 px-2 py-1 rounded-full">Online</span>
                          )}
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => copyUsername(order.user.ingame_name)}
                            className="h-6 w-6 p-0 ml-auto mr-3"
                          >
                            {copiedUser === order.user.ingame_name ? (
                              <Check className="w-4 h-4 text-orange-400" />
                            ) : (
                              <Copy className="w-4 h-4 text-gray-400 hover:text-gray-200" />
                            )}
                          </Button>
                        </div>
                      </div>
                      <div className="text-right">
                        <div className="text-2xl font-bold text-orange-400">{order.platinum}p</div>
                        <div className="text-xs text-gray-400">Qty: {order.quantity}</div>
                      </div>
                    </div>
                  ))
                ) : (
                  <div className="text-center py-8 text-gray-500">No buy orders available</div>
                )}
              </TabsContent>
            </Tabs>
          </div>
        )}

        {!selectedMod && !loading && (
          <div className="text-center text-gray-400 py-12">
            <div className="flex flex-col items-center gap-4">
              <div className="w-20 h-20 rounded-full bg-gradient-to-br from-blue-500/10 to-purple-500/10 flex items-center justify-center">
                <Search className="w-10 h-10 opacity-40" />
              </div>
              <div>
                <p className="text-lg font-medium text-gray-300 mb-2">Search for Mods & Items</p>
                <p className="text-sm text-gray-500 max-w-md mx-auto">
                  Enter an item name to view current market prices, trading orders, and detailed information
                </p>
              </div>
            </div>
          </div>
        )}
        
        {loading && (
          <div className="text-center text-gray-400 py-12">
            <div className="flex flex-col items-center gap-4">
              <div className="w-12 h-12 border-4 border-blue-500/30 border-t-blue-500 rounded-full animate-spin"></div>
              <p className="text-sm">Searching for "{searchQuery}"...</p>
            </div>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
