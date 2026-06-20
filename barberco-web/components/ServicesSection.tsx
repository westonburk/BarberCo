import Link from "next/link";
import { formatPrice } from "@/lib/format-price";
import { getServices } from "@/lib/get-services";

export async function ServicesSection() {
  const services = await getServices();

  return (
    <section className="px-6 pt-6 pb-12 md:pt-8 md:pb-16">
      <div className="mx-auto max-w-3xl">
        <header className="mb-10 text-center">
          <p className="mb-4 text-sm uppercase tracking-[0.3em] text-accent">
            Services
          </p>
          <h2 className="font-serif text-4xl tracking-wide md:text-5xl">
            The Menu
          </h2>
          <p className="mx-auto mt-4 max-w-md text-muted">
            Every service performed with intention — no rush, no compromise.
          </p>
        </header>

        <ul className="border-y border-foreground/10">
          {services.map((service) => (
            <li
              key={service.id}
              className="group border-b border-foreground/10 py-6 last:border-b-0"
            >
              <div className="flex items-baseline gap-4">
                <span className="font-serif text-xl tracking-wide transition-colors group-hover:text-accent md:text-2xl">
                  {service.name}
                </span>
                <span
                  className="mb-1.5 min-w-8 flex-1 border-b border-dotted border-foreground/20"
                  aria-hidden="true"
                />
                <span className="shrink-0 font-sans text-lg tabular-nums text-accent">
                  {formatPrice(service.price)}
                </span>
              </div>
            </li>
          ))}
        </ul>

        <p className="mt-8 text-center text-sm text-muted">
          Prices subject to change. Gratuity appreciated.
        </p>

        <div className="mt-12 text-center">
          <Link
            href="/appointment"
            className="inline-block border border-accent px-8 py-3 text-sm uppercase tracking-[0.2em] text-accent transition-colors hover:bg-accent hover:text-background"
          >
            Reserve Your Chair
          </Link>
        </div>
      </div>
    </section>
  );
}
