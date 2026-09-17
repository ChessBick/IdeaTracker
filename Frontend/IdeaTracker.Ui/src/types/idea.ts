export type IdeaStatus = "Proposed" | "InReview" | "Approved" | "Rejected";

export const IDEA_STATUSES: IdeaStatus[] = ["Proposed", "InReview", "Approved", "Rejected"];

export interface IdeaDto {
  id: number;
  title: string;
  description?: string | null;
  status: IdeaStatus;
  tags: string[];
  createdAt: string;
  updatedAt: string;
}

export interface CreateIdeaRequest {
  title: string;
  description?: string;
  tags?: string[];
}

export interface UpdateIdeaRequest {
  title: string;
  description?: string;
  status: IdeaStatus;
  tags?: string[];
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string | null;
}

export interface PaginationMeta {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface PagedResult<T> {
  items: T[];
  pagination: PaginationMeta;
}