import { Navbar } from "@/components/navbar";
import { SearchSection } from "@/components/search-section";
import { GameStatusSection } from "@/components/game-status-section";

export default function Home() {
  return (
    <main className="min-h-screen bg-gradient-to-br from-slate-950 via-blue-950 to-slate-950 relative overflow-hidden">
      {/* Background decorative elements */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className="absolute top-0 left-1/4 w-96 h-96 bg-blue-500/5 rounded-full blur-3xl"></div>
        <div className="absolute bottom-0 right-1/4 w-96 h-96 bg-purple-500/5 rounded-full blur-3xl"></div>
      </div>
      
      <Navbar />
      
      <div className="container mx-auto px-4 py-8 space-y-8 relative z-10">
        {/* Hero Section */}
        <div className="text-center space-y-4 py-12">
          <h1 className="text-5xl md:text-7xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-blue-400 via-purple-400 to-pink-400 animate-pulse">
            Warframe Utils
          </h1>
          <p className="text-xl text-gray-400 max-w-2xl mx-auto">
            Real-time market prices, trading orders, and game status monitoring
          </p>
        </div>

        {/* Game Status Cards */}
        <GameStatusSection />

        {/* Market Search */}
        <SearchSection />
      </div>
    </main>
  );
}
