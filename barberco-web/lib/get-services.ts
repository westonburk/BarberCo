import { apiFetch } from "./api-client";
import type { Service } from "./types";

export async function getServices(): Promise<Service[]> {
  return apiFetch<Service[]>("service");
}
