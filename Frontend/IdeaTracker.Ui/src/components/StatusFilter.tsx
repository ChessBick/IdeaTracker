import { IDEA_STATUSES, type IdeaStatus } from "../types/idea";

interface StatusFilterProps {
  value: IdeaStatus | "";
  onChange: (value: IdeaStatus | "") => void;
}

export function StatusFilter({ value, onChange }: StatusFilterProps) {
  return (
    <label className="status-filter">
      Status:
      <select value={value} onChange={(e) => onChange(e.target.value as IdeaStatus | "")}>
        <option value="">All</option>
        {IDEA_STATUSES.map((status) => (
          <option key={status} value={status}>
            {status}
          </option>
        ))}
      </select>
    </label>
  );
}
