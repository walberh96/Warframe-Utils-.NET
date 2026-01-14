/**
 * Get the base API URL
 * In production, this will use the Next.js rewrite proxy
 * In development, it can use NEXT_PUBLIC_API_URL or default to /api
 */
export function getApiUrl(): string {
  // Use environment variable if set, otherwise use relative path (Next.js rewrite)
  if (typeof window !== 'undefined') {
    // Client-side: use relative path (Next.js rewrites handle proxying)
    return '/api';
  }
  // Server-side: use environment variable or default
  return process.env.NEXT_PUBLIC_API_URL || process.env.BACKEND_URL || '/api';
}

/**
 * Build a full API endpoint URL
 */
export function apiUrl(endpoint: string): string {
  const baseUrl = getApiUrl();
  // Remove leading slash from endpoint if present to avoid double slashes
  const cleanEndpoint = endpoint.startsWith('/') ? endpoint.slice(1) : endpoint;
  return `${baseUrl}/${cleanEndpoint}`;
}

