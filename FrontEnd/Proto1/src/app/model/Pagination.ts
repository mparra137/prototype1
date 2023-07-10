export class Pagination {
    currentPage!: number;
    pageSize!: number;
    totalPages!: number;
    totalCount!: number;
}

export class PaginatedResult<T> {
    result!: T;
    pagination!: Pagination;
}
