namespace CompieTest_NoticeBoard.Server
{
    public sealed class NoticeBoardItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
