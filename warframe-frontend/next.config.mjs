/** @type {import('next').NextConfig} */
const nextConfig = {
  output: 'standalone', // Required for Docker deployment
  images: {
    domains: ['warframe.market'],
  },
  async rewrites() {
    // Use environment variable for backend URL, fallback to localhost for development
    const backendUrl = process.env.BACKEND_URL || 'http://localhost:5089';
    return [
      {
        source: '/api/:path*',
        destination: `${backendUrl}/api/:path*`, // Proxy to .NET backend
      },
      {
        source: '/Identity/:path*',
        destination: `${backendUrl}/Identity/:path*`, // Proxy Identity/Auth routes
      },
    ];
  },
};

export default nextConfig;
