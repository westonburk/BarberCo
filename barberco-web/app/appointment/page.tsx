import Image from "next/image";
import Link from "next/link";
import { AppointmentForm } from "@/components/AppointmentForm";
import { getServices } from "@/lib/get-services";

export default async function AppointmentPage() {
  const services = await getServices();

  return (
    <main className="min-h-screen">
      <div className="grid min-h-screen md:grid-cols-2">
        <div className="relative min-h-64 md:min-h-full">
          <Image
            src="/images/appointment.jpg"
            alt="Classic barbershop stations with brick walls and pendant lighting"
            fill
            className="object-cover"
            priority
          />
          <div className="image-vignette pointer-events-none absolute inset-0" />
          <div className="absolute inset-0 bg-gradient-to-t from-background via-background/20 to-transparent md:bg-gradient-to-r md:from-transparent md:via-background/30 md:to-background" />
        </div>

        <div className="flex flex-col justify-center px-6 py-16 md:px-12 md:py-24">
          <p className="mb-4 text-sm uppercase tracking-[0.3em] text-accent">
            Book
          </p>
          <h1 className="font-serif text-4xl tracking-wide md:text-5xl">
            Reserve Your Chair
          </h1>
          <p className="mt-4 max-w-md text-muted">
            Choose your services, pick a time, and we&apos;ll see you in the
            chair.
          </p>

          <AppointmentForm services={services} />

          <Link
            href="/"
            className="mt-8 text-sm text-muted transition-colors hover:text-accent"
          >
            &larr; Back to home
          </Link>
        </div>
      </div>
    </main>
  );
}
