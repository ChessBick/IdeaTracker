import { useState, type FormEvent } from "react";
import { IDEA_STATUSES, type IdeaDto, type IdeaStatus } from "../types/idea";

export interface IdeaFormValues {
  title: string;
  description: string;
  status: IdeaStatus;
  tags: string;
}

interface IdeaFormProps {
  idea?: IdeaDto;
  submitLabel: string;
  onSubmit: (values: IdeaFormValues) => Promise<void>;
  onCancel?: () => void;
}

export function IdeaForm({ idea, submitLabel, onSubmit, onCancel }: IdeaFormProps) {
  const [title, setTitle] = useState(idea?.title ?? "");
  const [description, setDescription] = useState(idea?.description ?? "");
  const [status, setStatus] = useState<IdeaStatus>(idea?.status ?? "Proposed");
  const [tags, setTags] = useState(idea?.tags.join(", ") ?? "");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();

    if (!title.trim()) {
      setError("Title is required.");
      return;
    }

    setError(null);
    setSubmitting(true);
    try {
      await onSubmit({ title: title.trim(), description, status, tags });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to save idea.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className="idea-form" onSubmit={handleSubmit}>
      <label>
        Title *
        <input value={title} onChange={(e) => setTitle(e.target.value)} maxLength={200} />
      </label>

      <label>
        Description
        <textarea value={description} onChange={(e) => setDescription(e.target.value)} rows={4} />
      </label>

      {idea && (
        <label>
          Status
          <select value={status} onChange={(e) => setStatus(e.target.value as IdeaStatus)}>
            {IDEA_STATUSES.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </select>
        </label>
      )}

      <label>
        Tags (comma separated)
        <input value={tags} onChange={(e) => setTags(e.target.value)} placeholder="e.g. growth, mobile" />
      </label>

      {error && <p className="form-error">{error}</p>}

      <div className="form-actions">
        <button type="submit" className="btn-primary" disabled={submitting}>
          {submitting ? "Saving..." : submitLabel}
        </button>
        {onCancel && (
          <button type="button" onClick={onCancel} disabled={submitting}>
            Cancel
          </button>
        )}
      </div>
    </form>
  );
}
