/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    domains: ['warframe.market'],
  },
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: 'http://localhost:5089/api/:path*', // Proxy to .NET backend
      },
      {
        source: '/Identity/:path*',
        destination: 'http://localhost:5089/Identity/:path*', // Proxy Identity/Auth routes
      },
    ];
  },
};

export default nextConfig;
