import { apiFetch, isApiConfigured } from "./api-client";
import { logApiFallback } from "./log-api-fallback";
import type { Service } from "./types";

const PLACEHOLDER_SERVICES: Service[] = [
  { id: 1, name: "Classic Haircut", price: 35 },
  { id: 2, name: "Skin Fade", price: 40 },
  { id: 3, name: "Hot Towel Shave", price: 45 },
  { id: 4, name: "Beard Trim & Shape", price: 25 },
  { id: 5, name: "Haircut & Beard", price: 55 },
  { id: 6, name: "Children's Cut", price: 22 },
  { id: 7, name: "Senior Cut", price: 28 },
  { id: 8, name: "Buzz Cut", price: 20 },
  { id: 9, name: "Line Up", price: 15 },
];

export async function getServices(): Promise<Service[]> {
  if (isApiConfigured()) {
    try {
      return await apiFetch<Service[]>("service");
    } catch (error) {
      logApiFallback("services", error);
    }
  }

  return PLACEHOLDER_SERVICES;
}
