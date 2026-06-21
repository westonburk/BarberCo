import Image from "next/image";
import Link from "next/link";

type LogoProps = {
  className?: string;
  size?: number;
};

export function Logo({ className = "", size = 36 }: LogoProps) {
  return (
    <Link
      href="/"
      className={`inline-flex shrink-0 items-center ${className}`}
      aria-label="Mike's Barber Shop home"
    >
      <Image
        src="/logo.svg"
        alt=""
        width={size}
        height={size}
        priority
        className="h-auto w-auto"
        sizes={`${size}px`}
      />
    </Link>
  );
}
