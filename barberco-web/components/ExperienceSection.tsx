import Image from "next/image";
import { OrnamentedHeading } from "@/components/OrnamentedHeading";

export function ExperienceSection() {
  return (
    <section className="px-6 py-12 md:py-16">
      <div className="mx-auto grid max-w-6xl items-center gap-12 md:grid-cols-2 md:gap-16">
        <div className="relative aspect-[4/5] md:order-1">
          <Image
            src="/images/craft.jpg"
            alt="Premium grooming products and barber tools arranged on a wooden surface"
            fill
            className="object-cover"
            sizes="(max-width: 768px) 100vw, 576px"
          />
          <div className="image-vignette pointer-events-none absolute inset-0" />
        </div>

        <div className="md:order-2">
          <OrnamentedHeading>Ritual, Not Routine</OrnamentedHeading>
          <p className="mt-6 leading-relaxed text-muted">
            From the moment you settle into the chair, the pace slows. A hot
            towel. A straight razor drawn with care. Products chosen for quality,
            not convenience.
          </p>
          <p className="mt-4 leading-relaxed text-muted">
            Mike&apos;s is built on the details most shops rush past — the
            neckline, the blend, the finish. It&apos;s grooming the way it was
            meant to be done: with patience, precision, and a little conversation.
          </p>

          <ul className="mt-8 space-y-3 border-t border-foreground/10 pt-8">
            {[
              "Hot towel treatment",
              "Straight-razor finish",
              "Premium grooming products",
              "Complimentary beverage",
            ].map((item) => (
              <li
                key={item}
                className="flex items-center gap-3 text-sm tracking-wide text-foreground/90"
              >
                <span className="h-px w-6 bg-accent" aria-hidden="true" />
                {item}
              </li>
            ))}
          </ul>
        </div>
      </div>
    </section>
  );
}
