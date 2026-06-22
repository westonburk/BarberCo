type HeadingOrnamentProps = {
  className?: string;
};

export function HeadingOrnament({ className = "" }: HeadingOrnamentProps) {
  return (
    <svg
      viewBox="0 0 280 18"
      className={`h-[18px] w-56 shrink-0 text-accent md:h-5 md:w-64${className ? ` ${className}` : ""}`}
      aria-hidden="true"
    >
      <circle cx="12" cy="9" r="2" fill="currentColor" />
      <circle cx="268" cy="9" r="2" fill="currentColor" />
      <line
        x1="16"
        y1="9"
        x2="264"
        y2="9"
        stroke="currentColor"
        strokeWidth="0.9"
        strokeLinecap="round"
      />
      <path fill="currentColor" d="M114 9l5-5 5 5-5 5z" />
      <path fill="currentColor" d="M129 9l7-7 7 7-7 7z" />
      <path fill="currentColor" d="M148 9l5-5 5 5-5 5z" />
    </svg>
  );
}
