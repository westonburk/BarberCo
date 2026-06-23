const LOCAL_DATETIME_PATTERN = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$/;

/** Local wall-clock time for the API, e.g. "2026-06-22T15:00:00" (no timezone suffix). */
export function formatLocalDateTime(date: Date): string {
  return new Date(date.getTime() - date.getTimezoneOffset() * 60_000)
    .toISOString()
    .slice(0, 19);
}

export function isLocalDateTimeString(value: string): boolean {
  return LOCAL_DATETIME_PATTERN.test(value);
}
