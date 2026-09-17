import type {
  ApiResponse,
  CreateIdeaRequest,
  IdeaDto,
  IdeaStatus,
  PagedResult,
  UpdateIdeaRequest,
} from "../types/idea";

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

async function handle<T>(res: Response): Promise<T> {
  if (!res.ok) throw new Error(`Request failed (${res.status})`);
  const body: ApiResponse<T> = await res.json();
  return body.data;
}

export interface ListIdeasParams {
  status?: IdeaStatus;
  page?: number;
  pageSize?: number;
}

export const ideasApi = {
  list: ({ status, page, pageSize }: ListIdeasParams = {}) => {
    const params = new URLSearchParams();
    if (status) params.set("status", status);
    if (page) params.set("page", String(page));
    if (pageSize) params.set("pageSize", String(pageSize));
    const query = params.toString();
    return fetch(`${BASE_URL}${query ? `?${query}` : ""}`).then((r) => handle<PagedResult<IdeaDto>>(r));
  },
  get: (id: number) => fetch(`${BASE_URL}/${id}`).then((r) => handle<IdeaDto>(r)),
  create: (req: CreateIdeaRequest) =>
    fetch(BASE_URL, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(req) })
      .then((r) => handle<IdeaDto>(r)),
  update: (id: number, req: UpdateIdeaRequest) =>
    fetch(`${BASE_URL}/${id}`, { method: "PUT", headers: { "Content-Type": "application/json" }, body: JSON.stringify(req) })
      .then((r) => handle<IdeaDto>(r)),
  remove: (id: number) =>
    fetch(`${BASE_URL}/${id}`, { method: "DELETE" }).then((r) => {
      if (!r.ok) throw new Error(`Delete failed (${r.status})`);
    }),
};