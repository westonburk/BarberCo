import { HeadingOrnament } from "@/components/HeadingOrnament";

type OrnamentedHeadingProps = {
  children: React.ReactNode;
  align?: "left" | "center";
  as?: "h1" | "h2";
  className?: string;
};

export function OrnamentedHeading({
  children,
  align = "left",
  as: Tag = "h2",
  className = "",
}: OrnamentedHeadingProps) {
  const alignClass =
    align === "center" ? "items-center text-center" : "items-start text-left";

  return (
    <div className={`mb-6 flex flex-col gap-3 ${alignClass} ${className}`.trim()}>
      <HeadingOrnament />
      <Tag className="font-serif text-4xl tracking-wide md:text-5xl">{children}</Tag>
    </div>
  );
}
