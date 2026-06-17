function isApiUnavailable(error: unknown): boolean {
  if (!(error instanceof Error)) {
    return false;
  }

  if (error.message.toLowerCase().includes("fetch failed")) {
    return true;
  }

  if ("cause" in error && error.cause) {
    return isApiUnavailable(error.cause);
  }

  return false;
}

export function logApiFallback(resource: string, error: unknown): void {
  if (process.env.NODE_ENV !== "development") {
    return;
  }

  if (isApiUnavailable(error)) {
    console.warn(`API unavailable for ${resource}, using placeholders.`);
    return;
  }

  console.error(`Failed to fetch ${resource} from API, using placeholders:`, error);
}
