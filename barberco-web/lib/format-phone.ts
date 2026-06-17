export function formatPhoneNumber(input: string): string {
  const digits = input.replace(/\D/g, "").slice(0, 10);

  if (digits.length === 0) {
    return "";
  }

  if (digits.length >= 6) {
    return `(${digits.slice(0, 3)}) ${digits.slice(3, 6)}-${digits.slice(6)}`;
  }

  if (digits.length >= 3) {
    return `(${digits.slice(0, 3)}) ${digits.slice(3)}`;
  }

  return `(${digits}`;
}

export const PHONE_PATTERN = /^\(\d{3}\) \d{3}-\d{4}$/;
