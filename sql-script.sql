CREATE TABLE [Book] (
	Id integer NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(255),
	Price float,
	NumOfPage integer,
	PublishingCompany nvarchar(255),
	Author nvarchar(255),
	Category_Id integer,
	Cover nvarchar(255),
	Description nvarchar(255),
)
GO
CREATE TABLE [Category] (
	Id integer NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(255),

)
GO
CREATE TABLE [Order] (
	Id integer NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CustomerName nvarchar(255), 
	TotalPrice float,
	CreatedAt datetime,
	Discount_Id integer,
)
GO
CREATE TABLE [Order_Book] (
	Order_Id integer  ,
	Book_Id integer ,
	NumOfBook integer ,
  CONSTRAINT [PK_ORDER_BOOK] PRIMARY KEY CLUSTERED
  (
  [Order_Id],
  [Book_Id]
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Discount] (
	Id integer NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Code nvarchar(255) ,
	DiscountPercent float ,
	MaxDiscount float ,
)
GO
ALTER TABLE [Book] WITH CHECK ADD CONSTRAINT [Book_fk0] FOREIGN KEY ([Category_Id]) REFERENCES [Category]([Id])
ON UPDATE CASCADE
ON DELETE SET NULL;
GO
ALTER TABLE [Book] CHECK CONSTRAINT [Book_fk0]
GO


ALTER TABLE [Order] WITH CHECK ADD CONSTRAINT [Order_fk0] FOREIGN KEY ([Discount_Id]) REFERENCES [Discount]([Id])
ON UPDATE CASCADE
ON DELETE SET NULL;
GO
ALTER TABLE [Order] CHECK CONSTRAINT [Order_fk0]
GO

ALTER TABLE [Order_Book] WITH CHECK ADD CONSTRAINT [Order_Book_fk0] FOREIGN KEY ([Order_Id]) REFERENCES [Order]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Order_Book] CHECK CONSTRAINT [Order_Book_fk0]
GO
ALTER TABLE [Order_Book] WITH CHECK ADD CONSTRAINT [Order_Book_fk1] FOREIGN KEY ([Book_Id]) REFERENCES [Book]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Order_Book] CHECK CONSTRAINT [Order_Book_fk1]
GO





INSERT INTO Discount (Code, DiscountPercent, MaxDiscount)
VALUES
	(N'HAPPYNEWYEAR',10,50000),
	(N'OPENING',15,30000),
	(N'BDAY',20,100000);

INSERT INTO Category (Name)
VALUES
	(N'Truyện Trinh thám'),
	(N'Sách Văn học'),
	(N'Truyện Kinh dị');

INSERT INTO Book (Name, Price, NumOfPage, PublishingCompany, Author, Category_Id, Cover, Description)
VALUES
    (N'Rừng than khóc', 189000, 488, N'NXB Thanh Niên', N'Thục Linh', 1,N'/Resources/BookCovers/rung-than-khoc.jpg',N'Description...'),
    (N'Fourth Wing', 315000, 824, N'NXB Trẻ', N'Rebecca Yarros', 2,N'/Resources/BookCovers/canh-tu.jpg', N'Description...'),
    (N'Cái chết hạnh phúc', 98000, 216, N'NXB Văn Học', N'Albert Camus', 1, N'/Resources/BookCovers/cai-chet-hanh-phuc.jpg', N'Description...'),
    (N'Thần thoại Celt', 175000, 323, N'NXB Dân Trí', N'Albert Camus', 1, N'/Resources/BookCovers/than-thoai-celt.jpg', N'Description...'),
    (N'Chim ruồi rực cháy', 199000, 464, N'NXB Văn học', N'Tân Minh', 3, N'/Resources/BookCovers/than-thoai-celt.jpg', N'Description...'),
    (N'Chim ruồi rực cháy', 199000, 464, N'NXB Văn học', N'Tân Minh', 3, N'/Resources/BookCovers/chim-ruoi-ruc-chay.jpg', N'Description...'),
    (N'Trở lại hiện trường vụ án', 165000, 387, N'NXB Văn học', N'Mã Định Hàn Trang', 3, N'/Resources/BookCovers/tro-lai-hien-truong-vu-an.jpg', N'Description...'),
    (N'Những cô gái cuối cùng', 199000, 256, N'NXB Lao Động', N'Riley Sager', 3, N'/Resources/BookCovers/nhung-co-gai-cuoi-cung.jpg', N'Description...'),
    (N'Những hành tinh của Robin', 179000, 425, N'NXB Văn học', N'Richard Powers', 1, N'/Resources/BookCovers/nhung-hanh-tinh-cua-robin.jpg', N'Description...'),
    (N'Giận', 129000, 225, N'NXB Thế Giới', N'Thích Nhất Hạnh', 1, N'/Resources/BookCovers/gian.jpg', N'Description...'),
    (N'Hội cố thi nhân', 149000, 275, N'NXB Thanh Niên', N'Nancy H. Kleinbaum', 2, N'/Resources/BookCovers/hoi-co-thi-nhan.jpg', N'Description...'),
    (N'Dê mặt quỷ', 249000, 556, N'NXB Thanh Niên', N'Chan-Ho-Kei', 2, N'/Resources/BookCovers/de-mat-quy.jpg', N'Description...'),
    (N'Juliette Và Tiệm Sách Bí Ẩn Ở Paris', 159000, 284, N'NXB Thanh Niên', N'Christine Féret-Fleury', 1, N'/Resources/BookCovers/juliette-va-tiem-sach-bi-an-o-paris.jpg', N'Description...'),
    (N'Lạc Vào Vùng Ký Ức', 179000, 456, N'NXB Thanh Niên', N'Sarah Addison Allen', 2, N'/Resources/BookCovers/lac-vao-vung-ky-uc.jpg', N'Description...'),
    (N'Biến Thể Của Cô Đơn', 70000, 156, N'NXB Trẻ', N'Yang Phan', 2, N'/Resources/BookCovers/bien-the-cua-co-don.jpg', N'Description...'),
    (N'Những Quân Bài Trên Mặt Bàn', 170000, 316, N'NXB Trẻ', N'Agatha Christie', 2, N'/Resources/BookCovers/nhung-quan-bai-tren-mat-ban.jpg', N'Description...'),
    (N'Đảo chìm', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/dao-chim.jpg', N'Description...'),
    (N'Sự Kiên Định Của Kẻ Săn Mồi', 170000, 216, N'NXB Thanh Niên', N'Maxime Chattam', 1, N'/Resources/BookCovers/su-kien-dinh-cua-ke-san-moi.jpg', N'Description...'),
    (N'Nàng Cựu Idol Lớp Tôi Lại Có Hành Động Đáng Ngờ Nữa Rồi - Tập 2', 149000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/nang-cuu-idol-lop-toi-lai-co-hanh-dong-dang-ngo-nua-roi-tap-2.jpg', N'Description...'),
    (N'Xa Một Chút Hơn Một Triệu Năm Ánh Sáng', 150000, 216, N'NXB Hà Nội', N'Hideyuki Furuhashi', 1, N'/Resources/BookCovers/xa-mot-chut-hon-mot-trieu-nam-anh-sang.jpg', N'Description...'),
    (N'Ác Duyên - Duyên Khởi', 155000, 316, N'NXB Hội Nhà Văn', N'Rosie Nguyễn', 1, N'/Resources/BookCovers/ac-duyen.jpg', N'Description...'),
    (N'Chương Cuối Của Mùa Hạ', 120000, 216, N'NXB Hội Nhà Văn', N'Nam Thanh', 1, N'/Resources/BookCovers/chuong-cuoi-cua-mua-ha.jpg', N'Description...'),
    (N'Phố Vẫn Gió', 168000, 288, N'NXB Hội Nhà Văn', N'Lê Minh Hà', 1, N'/Resources/BookCovers/pho-van-gio.jpg', N'Description...'),
    (N'Gió Tự Thời Khuất Mặt', 172000, 216, N'NXB Hội Nhà Văn', N'Lê Minh Hà', 1, N'/Resources/BookCovers/gio-tu-thoi-khuat-mat.jpg', N'Description...'),
    (N'W Hay Là Ký Ức Tuổi Thơ', 110000, 216, N'NXB Hội Nhà Văn', N'Georges Perec', 1, N'/Resources/BookCovers/w-hay-la-ky-uc-tuoi-tho.jpg', N'Description...'),
    (N'Những Kẻ Ăn Sách', 230000, 216, N'NXB Thanh Niên', N'Sunyi Dean', 1, N'/Resources/BookCovers/nhung-ke-an-sach.jpg', N'Description...'),
    (N'Nỗi Cô Đơn Của Các Số Nguyên Tố', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/noi-co-don-cua-cac-so-nguyen-to.jpg', N'Description...'),
    (N'Vụ Án Sôcôla Có Độc', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/vu-an-socola-co-doc.jpg', N'Description...'),
    (N'Bản Du Ca Cuối Cùng', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/ban-du-ca-cuoi-cung.jpg', N'Description...'),
    (N'17 Âm 1', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/17-am-1.jpg', N'Description...'),
    (N'Ngủ Giấc Ngàn Thu', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/ngu-giac-ngan-thu.jpg', N'Description...'),
    (N'Thế Giới Atlantis', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/the-gioi-atlantis.jpg', N'Description...'),
    (N'Chitose Trong Chai Ramune - Tập 5', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/chitose-trong-chai-ramune-tap-5.jpg', N'Description...'),
    (N'Bác Hana', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/bac-hana.jpg', N'Description...'),
    (N'Xoắn Ốc Vô Hình', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/xoan-oc-vo-hinh.jpg', N'Description...')
