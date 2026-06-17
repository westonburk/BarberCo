import Image from "next/image";
import Link from "next/link";
import { ExperienceSection } from "@/components/ExperienceSection";
import { GallerySection } from "@/components/GallerySection";
import { HoursLocationSection } from "@/components/HoursLocationSection";
import { ServicesSection } from "@/components/ServicesSection";

export default function Home() {
  return (
    <main>
      <section className="relative flex min-h-screen items-center justify-center overflow-hidden">
        <Image
          src="/images/hero.jpg"
          alt="Classic barber chairs in a moody barbershop interior"
          fill
          className="object-cover"
          priority
        />
        <div className="absolute inset-0 bg-gradient-to-t from-background via-background/80 to-background/40" />
        <div className="relative z-10 px-6 text-center">
          <p className="mb-4 text-sm uppercase tracking-[0.3em] text-accent">
            Est. Tradition
          </p>
          <h1 className="font-serif text-5xl tracking-wide md:text-7xl">
            Mike&apos;s Barber Shop
          </h1>
          <p className="mx-auto mt-6 max-w-md text-lg text-muted">
            Where craftsmanship meets luxury.
          </p>
          <Link
            href="/appointment"
            className="mt-10 inline-block border border-accent px-8 py-3 text-sm uppercase tracking-[0.2em] text-accent transition-colors hover:bg-accent hover:text-background"
          >
            Reserve Your Chair
          </Link>
        </div>
      </section>

      <section className="relative -mt-32 px-6 pb-24 pt-8 md:pb-32 md:pt-12">
        <div className="mx-auto grid max-w-6xl items-center gap-12 md:grid-cols-2 md:gap-16">
          <div>
            <p className="mb-4 text-sm uppercase tracking-[0.3em] text-accent">
              The House
            </p>
            <h2 className="font-serif text-4xl tracking-wide md:text-5xl">
              More Than a Haircut
            </h2>
            <p className="mt-6 leading-relaxed text-muted">
              Mike&apos;s has been a neighborhood institution for over fifty
              years — timeless cuts, hot towel shaves, and the kind of
              conversation you can&apos;t rush. Founded by master barber Mike
              Chapman, the shop pairs old-world technique with an atmosphere
              that feels deliberate, unhurried, and entirely its own.
            </p>
            <p className="mt-4 leading-relaxed text-muted">
              Whether you want a straight-razor finish or a modern fade, every
              visit is treated as craft — not a transaction.
            </p>
          </div>
          <div className="relative aspect-[4/5]">
            <Image
              src="/images/service.jpg"
              alt="A precise beard trim in a classic barbershop"
              fill
              className="object-cover object-[center_25%]"
            />
            <div className="image-vignette pointer-events-none absolute inset-0" />
          </div>
        </div>
      </section>

      <ServicesSection />

      <ExperienceSection />

      <GallerySection />

      <HoursLocationSection />
    </main>
  );
}
