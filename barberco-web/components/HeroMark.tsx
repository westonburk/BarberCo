type HeroMarkProps = {
  className?: string;
};

export function HeroMark({
  className = "mx-auto mb-8 h-20 w-auto text-accent md:h-24",
}: HeroMarkProps) {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 160 72"
      fill="none"
      role="img"
      aria-label="Mike's Barber Shop"
      className={className}
    >
      <path
        stroke="currentColor"
        strokeWidth="1"
        strokeLinecap="square"
        d="M8 36h36M116 36h36"
        opacity="0.45"
      />
      <path
        fill="currentColor"
        d="M48 62V14h6.5l27.5 42L109.5 14H116v48h-6V24.5L81.5 58 54 24.5V62H48z"
      />
      <path
        fill="currentColor"
        opacity="0.85"
        d="M44 14h10v3H44v-3zm62 0h10v3h-10v-3zM44 62h10v3H44v-3zm62 0h10v3h-10v-3z"
      />
    </svg>
  );
}
