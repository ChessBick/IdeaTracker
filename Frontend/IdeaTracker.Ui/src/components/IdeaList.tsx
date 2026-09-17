import { useEffect, useState } from "react";
import { ideasApi } from "../api/ideasApi";
import type { IdeaDto, IdeaStatus, PaginationMeta } from "../types/idea";
import { StatusFilter } from "./StatusFilter";

const PAGE_SIZE = 10;

const STATUS_BADGE_CLASS: Record<IdeaStatus, string> = {
  Proposed: "badge-proposed",
  InReview: "badge-inreview",
  Approved: "badge-approved",
  Rejected: "badge-rejected",
};

interface IdeaListProps {
  onEdit: (idea: IdeaDto) => void;
  reloadToken: number;
}

export function IdeaList({ onEdit, reloadToken }: IdeaListProps) {
  const [status, setStatus] = useState<IdeaStatus | "">("");
  const [page, setPage] = useState(1);
  const [ideas, setIdeas] = useState<IdeaDto[]>([]);
  const [pagination, setPagination] = useState<PaginationMeta | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  function handleStatusChange(next: IdeaStatus | "") {
    setStatus(next);
    setPage(1);
  }

  useEffect(() => {
    let cancelled = false;

    setLoading(true);
    setError(null);
    ideasApi
      .list({ status: status || undefined, page, pageSize: PAGE_SIZE })
      .then((result) => {
        if (cancelled) return;
        setIdeas(result.items);
        setPagination(result.pagination);
      })
      .catch((err) => {
        if (!cancelled) setError(err instanceof Error ? err.message : "Failed to load ideas.");
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => {
      cancelled = true;
    };
  }, [status, page, reloadToken]);

  async function handleDelete(id: number) {
    setDeletingId(id);
    try {
      await ideasApi.remove(id);
      setIdeas((prev) => prev.filter((i) => i.id !== id));
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete idea.");
    } finally {
      setDeletingId(null);
    }
  }

  return (
    <div className="idea-list">
      <StatusFilter value={status} onChange={handleStatusChange} />

      {loading && <p className="loading-state">Loading ideas...</p>}
      {error && <p className="form-error">{error}</p>}

      {!loading && !error && ideas.length === 0 && <p className="empty-state">No ideas found.</p>}

      {!loading && !error && ideas.length > 0 && (
        <div className="table-wrapper">
          <table className="idea-table">
            <thead>
              <tr>
                <th>Title</th>
                <th>Status</th>
                <th>Tags</th>
                <th>Updated</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {ideas.map((idea) => (
                <tr key={idea.id}>
                  <td>
                    <strong>{idea.title}</strong>
                    {idea.description && <p className="description">{idea.description}</p>}
                  </td>
                  <td>
                    <span className={`badge ${STATUS_BADGE_CLASS[idea.status]}`}>{idea.status}</span>
                  </td>
                  <td className="tags">{idea.tags.join(", ")}</td>
                  <td>{new Date(idea.updatedAt).toLocaleDateString()}</td>
                  <td className="idea-item-actions">
                    <button type="button" onClick={() => onEdit(idea)}>
                      Edit
                    </button>
                    <button
                      type="button"
                      className="btn-danger"
                      onClick={() => handleDelete(idea.id)}
                      disabled={deletingId === idea.id}
                    >
                      {deletingId === idea.id ? "Deleting..." : "Delete"}
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {pagination && pagination.totalPages > 1 && (
        <div className="pagination">
          <button type="button" onClick={() => setPage((p) => p - 1)} disabled={!pagination.hasPreviousPage}>
            Previous
          </button>
          <span>
            Page {pagination.page} of {pagination.totalPages}
          </span>
          <button type="button" onClick={() => setPage((p) => p + 1)} disabled={!pagination.hasNextPage}>
            Next
          </button>
        </div>
      )}
    </div>
  );
}

