export class ApiError extends Error {
  constructor(
    public status: number,
    message: string,
  ) {
    super(message);
    this.name = "ApiError";
  }
}

function getApiConfig() {
  const baseUrl = process.env.API_BASE_URL;
  const apiKey = process.env.API_KEY;

  if (!baseUrl || !apiKey) {
    return null;
  }

  return { baseUrl: baseUrl.replace(/\/$/, ""), apiKey };
}

export function isApiConfigured(): boolean {
  return getApiConfig() !== null;
}

export async function apiFetch<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const config = getApiConfig();
  if (!config) {
    throw new ApiError(0, "API is not configured");
  }

  const url = `${config.baseUrl}/${path.replace(/^\//, "")}`;
  const response = await fetch(url, {
    ...options,
    headers: {
      ApiKey: config.apiKey,
      "Content-Type": "application/json",
      ...options.headers,
    },
    cache: "no-store",
  });

  if (!response.ok) {
    const message = await response.text();
    throw new ApiError(response.status, message || response.statusText);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}
