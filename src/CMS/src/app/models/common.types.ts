export interface CategoryListItem{
  id: number;
  name: string;
  newsCount: number;
}

export interface BannerListItem{
  id: number;
  title: string;
  link: string;
  imageUrl: string;
  position: number;
}

export interface ArticleListItem{
  id: number;
  title: string;
  author: string;
  cover: string;
  categoryName: string;
  isHot: boolean;
  content: string;
  publishDate: string;
  publisherName: string;
  likeCount: string;
  dislikeCount: string;
  collectCount: string;
}
