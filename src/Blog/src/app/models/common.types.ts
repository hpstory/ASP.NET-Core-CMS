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
  likeCount: number;
  dislikeCount: number;
  collectCount: number;
}

export interface BannerListItem{
  id: number;
  title: string;
  link: string;
  imageUrl: string;
  position: number;
}
