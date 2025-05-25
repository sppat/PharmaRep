export default interface PaginatedResponse<T> {
  items: T[];
  total: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}
