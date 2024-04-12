
CREATE DATABASE DoAn1WindowsDb
GO
USE DoAn1WindowsDb
GO
CREATE TABLE [Book] (
	Id integer NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(255),
	Price float,
	NumOfPage integer,
	PublishingCompany nvarchar(255),
	Author nvarchar(255),
	Cover nvarchar(255),
	CostPrice float,
	Description nvarchar(255),
  Quantity integer,
	Category_Id integer,
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
	ShippingAddress nvarchar(255),
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
	(N'None',0,50000),
	(N'HAPPYNEWYEAR',10,50000),
	(N'OPENING',15,30000),
	(N'BDAY',20,100000);

INSERT INTO Category (Name)
VALUES
	(N'Truyện Trinh thám'),
	(N'Sách Văn học'),
	(N'Truyện Kinh dị');

INSERT INTO Book (Name, Price, NumOfPage, PublishingCompany, Author, Category_Id, Cover, Description, Quantity, CostPrice)
VALUES
    (N'Rừng than khóc', 189000, 488, N'NXB Thanh Niên', N'Thục Linh', 1,N'/Resources/BookCovers/rung-than-khoc.jpg',N'Description...', 1000, 89000),
    (N'Fourth Wing', 315000, 824, N'NXB Trẻ', N'Rebecca Yarros', 2,N'/Resources/BookCovers/canh-tu.jpg', N'Description...', 1000, 215000),
    (N'Cái chết hạnh phúc', 98000, 216, N'NXB Văn Học', N'Albert Camus', 1, N'/Resources/BookCovers/cai-chet-hanh-phuc.jpg', N'Description...', 1000, 58000),
    (N'Thần thoại Celt', 175000, 323, N'NXB Dân Trí', N'Albert Camus', 1, N'/Resources/BookCovers/than-thoai-celt.jpg', N'Description...', 1000, 125000),
    (N'Chim ruồi rực cháy', 199000, 464, N'NXB Văn học', N'Tân Minh', 3, N'/Resources/BookCovers/than-thoai-celt.jpg', N'Description...', 1000, 100000),
    (N'Chim ruồi rực cháy', 199000, 464, N'NXB Văn học', N'Tân Minh', 3, N'/Resources/BookCovers/chim-ruoi-ruc-chay.jpg', N'Description...', 1000, 100000),
    (N'Trở lại hiện trường vụ án', 165000, 387, N'NXB Văn học', N'Mã Định Hàn Trang', 3, N'/Resources/BookCovers/tro-lai-hien-truong-vu-an.jpg', N'Description...', 1000, 90000),
    (N'Những cô gái cuối cùng', 199000, 256, N'NXB Lao Động', N'Riley Sager', 3, N'/Resources/BookCovers/nhung-co-gai-cuoi-cung.jpg', N'Description...', 1000, 100000),
    (N'Những hành tinh của Robin', 179000, 425, N'NXB Văn học', N'Richard Powers', 1, N'/Resources/BookCovers/nhung-hanh-tinh-cua-robin.jpg', N'Description...', 1000, 100000),
    (N'Giận', 129000, 225, N'NXB Thế Giới', N'Thích Nhất Hạnh', 1, N'/Resources/BookCovers/gian.jpg', N'Description...', 1000, 100000),
    (N'Hội cố thi nhân', 149000, 275, N'NXB Thanh Niên', N'Nancy H. Kleinbaum', 2, N'/Resources/BookCovers/hoi-co-thi-nhan.jpg', N'Description...', 1000, 100000),
    (N'Dê mặt quỷ', 249000, 556, N'NXB Thanh Niên', N'Chan-Ho-Kei', 2, N'/Resources/BookCovers/de-mat-quy.jpg', N'Description...', 1000, 100000),
    (N'Juliette Và Tiệm Sách Bí Ẩn Ở Paris', 159000, 284, N'NXB Thanh Niên', N'Christine Féret-Fleury', 1, N'/Resources/BookCovers/juliette-va-tiem-sach-bi-an-o-paris.jpg', N'Description...', 1000, 100000),
    (N'Lạc Vào Vùng Ký Ức', 179000, 456, N'NXB Thanh Niên', N'Sarah Addison Allen', 2, N'/Resources/BookCovers/lac-vao-vung-ky-uc.jpg', N'Description...', 1000, 100000),
    (N'Biến Thể Của Cô Đơn', 70000, 156, N'NXB Trẻ', N'Yang Phan', 2, N'/Resources/BookCovers/bien-the-cua-co-don.jpg', N'Description...', 1000, 50000),
    (N'Những Quân Bài Trên Mặt Bàn', 170000, 316, N'NXB Trẻ', N'Agatha Christie', 2, N'/Resources/BookCovers/nhung-quan-bai-tren-mat-ban.jpg', N'Description...', 1000, 100000),
    (N'Đảo chìm', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/dao-chim.jpg', N'Description...', 1000, 70000),
    (N'Sự Kiên Định Của Kẻ Săn Mồi', 170000, 216, N'NXB Thanh Niên', N'Maxime Chattam', 1, N'/Resources/BookCovers/su-kien-dinh-cua-ke-san-moi.jpg', N'Description...', 1000, 100000),
    (N'Nàng Cựu Idol Lớp Tôi Lại Có Hành Động Đáng Ngờ Nữa Rồi - Tập 2', 149000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/nang-cuu-idol-lop-toi-lai-co-hanh-dong-dang-ngo-nua-roi-tap-2.jpg', N'Description...', 1000, 100000),
    (N'Xa Một Chút Hơn Một Triệu Năm Ánh Sáng', 150000, 216, N'NXB Hà Nội', N'Hideyuki Furuhashi', 1, N'/Resources/BookCovers/xa-mot-chut-hon-mot-trieu-nam-anh-sang.jpg', N'Description...', 1000, 100000),
    (N'Ác Duyên - Duyên Khởi', 155000, 316, N'NXB Hội Nhà Văn', N'Rosie Nguyễn', 1, N'/Resources/BookCovers/ac-duyen.jpg', N'Description...', 1000, 100000),
    (N'Chương Cuối Của Mùa Hạ', 120000, 216, N'NXB Hội Nhà Văn', N'Nam Thanh', 1, N'/Resources/BookCovers/chuong-cuoi-cua-mua-ha.jpg', N'Description...', 1000, 100000),
    (N'Phố Vẫn Gió', 168000, 288, N'NXB Hội Nhà Văn', N'Lê Minh Hà', 1, N'/Resources/BookCovers/pho-van-gio.jpg', N'Description...', 1000, 100000),
    (N'Gió Tự Thời Khuất Mặt', 172000, 216, N'NXB Hội Nhà Văn', N'Lê Minh Hà', 1, N'/Resources/BookCovers/gio-tu-thoi-khuat-mat.jpg', N'Description...', 1000, 100000),
    (N'W Hay Là Ký Ức Tuổi Thơ', 110000, 216, N'NXB Hội Nhà Văn', N'Georges Perec', 1, N'/Resources/BookCovers/w-hay-la-ky-uc-tuoi-tho.jpg', N'Description...', 1000, 100000),
    (N'Những Kẻ Ăn Sách', 230000, 216, N'NXB Thanh Niên', N'Sunyi Dean', 1, N'/Resources/BookCovers/nhung-ke-an-sach.jpg', N'Description...', 1000, 100000),
    (N'Nỗi Cô Đơn Của Các Số Nguyên Tố', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/noi-co-don-cua-cac-so-nguyen-to.jpg', N'Description...', 1000, 70000),
    (N'Vụ Án Sôcôla Có Độc', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/vu-an-socola-co-doc.jpg', N'Description...', 1000, 70000),
    (N'Bản Du Ca Cuối Cùng', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/ban-du-ca-cuoi-cung.jpg', N'Description...', 1000, 70000),
    (N'17 Âm 1', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/17-am-1.jpg', N'Description...', 1000, 70000),
    (N'Ngủ Giấc Ngàn Thu', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/ngu-giac-ngan-thu.jpg', N'Description...', 1000, 70000),
    (N'Thế Giới Atlantis', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/the-gioi-atlantis.jpg', N'Description...', 1000, 70000),
    (N'Chitose Trong Chai Ramune - Tập 5', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/chitose-trong-chai-ramune-tap-5.jpg', N'Description...', 1000, 70000),
    (N'Bác Hana', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/bac-hana.jpg', N'Description...', 1000, 70000),
    (N'Xoắn Ốc Vô Hình', 100000, 216, N'NXB Hội Nhà Văn', N'Trần Đăng Khoa', 1, N'/Resources/BookCovers/xoan-oc-vo-hinh.jpg', N'Description...', 1000, 70000)

INSERT INTO [Order] (CustomerName, ShippingAddress, TotalPrice, CreatedAt, Discount_Id)
VALUES
    (N'Nguyễn Văn A', N'Số 10, Đường Nguyễn Văn B, Phường Tân Định, Quận 1', 250000, '2024-03-20 08:30:00', 1),
    (N'Nguyễn Thị B', N'Số 20, Đường Lê Lai, Phường Bến Thành, Quận 1', 450000, '2024-03-21 09:45:00', 2),
    (N'Trần Văn C', N'Số 30, Đường Phan Văn Hân, Phường 17, Quận Bình Thạnh', 150000, '2024-03-22 10:20:00', 3),
    (N'Lê Thị D', N'Số 40, Đường Lê Quang Định, Phường 14, Quận Bình Thạnh', 300000, '2024-03-23 11:00:00', 1),
    (N'Phạm Văn E', N'Số 50, Đường Cách Mạng Tháng Tám, Phường 6, Quận 3', 200000, '2024-03-24 13:30:00', 2),
    (N'Huỳnh Thị F', N'Số 60, Đường Trần Hưng Đạo, Phường Phạm Ngũ Lão, Quận 1', 350000, '2024-03-25 15:15:00', 3),
    (N'Trần Văn G', N'Số 70, Đường Hoàng Sa, Phường Tân Định, Quận 1', 400000, '2024-03-26 17:45:00', 1),
    (N'Nguyễn Thanh I', N'Số 80, Đường Phạm Viết Chánh, Phường 19, Quận Bình Thạnh', 180000, '2024-03-28 20:00:00', 3),
    (N'Huỳnh Thị F', N'Số 90, Đường Đề Thám, Phường Phạm Ngũ Lão, Quận 1', 350000, '2024-03-25 15:15:00', 3),
    (N'Trần Văn G', N'Số 100, Đường Lê Thánh Tôn, Phường Bến Nghé, Quận 1', 400000, '2024-03-26 17:45:00', 1),
    (N'Nguyễn Thanh I', N'Số 110, Đường Võ Văn Kiệt, Phường Cầu Ông Lãnh, Quận 1', 180000, '2024-03-28 20:00:00', 3),
    (N'Huỳnh Thị F', N'Số 120, Đường Nguyễn Cư Trinh, Phường Nguyễn Cư Trinh, Quận 1', 350000, '2024-03-25 15:15:00', 3),
    (N'Trần Văn G', N'Số 130, Đường Hồ Tùng Mậu, Phường Bến Nghé, Quận 1', 400000, '2024-03-26 17:45:00', 1),
    (N'Nguyễn Thanh I', N'Số 140, Đường Bùi Thị Xuân, Phường Phạm Ngũ Lão, Quận 1', 180000, '2024-03-28 20:00:00', 3),
    (N'Lê Văn H', N'Số 150, Đường Nguyễn Trãi, Phường Bến Thành, Quận 1', 270000, '2024-03-27 18:30:00', 2);

INSERT INTO [Order_Book] (Order_Id, Book_Id, NumOfBook)
VALUES
    (1, 1, 1),
    (1, 2, 2),
    (2, 3, 1),
    (2, 4, 1),
    (2, 5, 1),
    (3, 6, 2),
    (3, 7, 1),
    (3, 8, 3),
    (4, 9, 1),
    (4, 10, 2),
    (5, 11, 1),
    (5, 12, 1),
    (5, 13, 1),
    (6, 14, 2),
    (6, 15, 1),
    (6, 16, 3),
    (7, 17, 1),
    (7, 18, 2),
    (8, 19, 1),
    (8, 20, 1),
    (8, 21, 1),
    (9, 22, 2),
    (9, 23, 1),
    (9, 24, 3);

USE DoAn1WindowsDb
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Văn A', N'Số 10, Đường Nguyễn Văn B, Phường Tân Định, Quận 1', 650000, CAST(N'2024-03-20T08:30:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thị B', N'Số 20, Đường Lê Lai, Phường Bến Thành, Quận 1', 750000, CAST(N'2024-03-21T09:45:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn C', N'Số 30, Đường Phan Văn Hân, Phường 17, Quận Bình Thạnh', 590000, CAST(N'2024-03-22T10:20:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Thị D', N'Số 40, Đường Lê Quang Định, Phường 14, Quận Bình Thạnh', 800000, CAST(N'2024-03-23T11:00:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Phạm Văn E', N'Số 50, Đường Cách Mạng Tháng Tám, Phường 6, Quận 3', 550000, CAST(N'2024-03-24T13:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị F', N'Số 60, Đường Trần Hưng Đạo, Phường Phạm Ngũ Lão, Quận 1', 750000, CAST(N'2024-03-25T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn G', N'Số 70, Đường Hoàng Sa, Phường Tân Định, Quận 1', 600000, CAST(N'2024-03-26T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh I', N'Số 80, Đường Phạm Viết Chánh, Phường 19, Quận Bình Thạnh', 675000, CAST(N'2024-03-28T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị F', N'Số 90, Đường Đề Thám, Phường Phạm Ngũ Lão, Quận 1', 650000, CAST(N'2024-03-25T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn G', N'Số 100, Đường Lê Thánh Tôn, Phường Bến Nghé, Quận 1', 400000, CAST(N'2024-03-26T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh I', N'Số 110, Đường Võ Văn Kiệt, Phường Cầu Ông Lãnh, Quận 1', 880000, CAST(N'2024-03-28T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị F', N'Số 120, Đường Nguyễn Cư Trinh, Phường Nguyễn Cư Trinh, Quận 1', 750000, CAST(N'2024-03-25T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn G', N'Số 130, Đường Hồ Tùng Mậu, Phường Bến Nghé, Quận 1', 500000, CAST(N'2024-03-26T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh I', N'Số 140, Đường Bùi Thị Xuân, Phường Phạm Ngũ Lão, Quận 1', 480000, CAST(N'2024-03-28T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Văn H', N'Số 150, Đường Nguyễn Trãi, Phường Bến Thành, Quận 1', 670000, CAST(N'2024-03-27T18:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Hải Anh', N'Số 10, Đường Nguyễn Văn B, Phường Tân Định, Quận 1', 800000, CAST(N'2024-04-01T08:30:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Hoàng Thị Lan', N'Số 20, Đường Lê Lai, Phường Bến Thành, Quận 1', 600000, CAST(N'2024-04-02T09:45:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Văn Hưng', N'Số 30, Đường Phan Văn Hân, Phường 17, Quận Bình Thạnh', 950000, CAST(N'2024-04-03T10:20:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Phạm Thanh Tuấn', N'Số 40, Đường Lê Quang Định, Phường 14, Quận Bình Thạnh', 550000, CAST(N'2024-04-04T11:00:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Hồ Thị Hằng', N'Số 50, Đường Cách Mạng Tháng Tám, Phường 6, Quận 3', 670000, CAST(N'2024-04-05T13:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Minh Thu', N'Số 60, Đường Trần Hưng Đạo, Phường Phạm Ngũ Lão, Quận 1', 625000, CAST(N'2024-04-06T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn Nam', N'Số 70, Đường Hoàng Sa, Phường Tân Định, Quận 1', 520000, CAST(N'2024-04-07T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thị Ngọc Anh', N'Số 80, Đường Phạm Viết Chánh, Phường 19, Quận Bình Thạnh', 720000, CAST(N'2024-04-08T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Hải Anh', N'Số 10, Đường Nguyễn Văn B, Phường Tân Định, Quận 1', 800000, CAST(N'2024-01-01T08:30:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Hoàng Thị Lan', N'Số 20, Đường Lê Lai, Phường Bến Thành, Quận 1', 600000, CAST(N'2024-01-02T09:45:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Văn Hưng', N'Số 30, Đường Phan Văn Hân, Phường 17, Quận Bình Thạnh', 950000, CAST(N'2024-01-03T10:20:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Phạm Thanh Tuấn', N'Số 40, Đường Lê Quang Định, Phường 14, Quận Bình Thạnh', 550000, CAST(N'2024-01-04T11:00:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Hồ Thị Hằng', N'Số 50, Đường Cách Mạng Tháng Tám, Phường 6, Quận 3', 500000, CAST(N'2024-01-05T13:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Minh Thu', N'Số 60, Đường Trần Hưng Đạo, Phường Phạm Ngũ Lão, Quận 1', 625000, CAST(N'2024-01-06T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn Nam', N'Số 70, Đường Hoàng Sa, Phường Tân Định, Quận 1', 570000, CAST(N'2024-01-07T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thị Ngọc Anh', N'Số 80, Đường Phạm Viết Chánh, Phường 19, Quận Bình Thạnh', 620000, CAST(N'2024-01-08T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Văn Lợi', N'Số 90, Đường Đề Thám, Phường Phạm Ngũ Lão, Quận 1', 870000, CAST(N'2024-01-09T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Thị Hương', N'Số 100, Đường Lê Thánh Tôn, Phường Bến Nghé, Quận 1', 590000, CAST(N'2024-01-10T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Hồng Sơn', N'Số 110, Đường Võ Văn Kiệt, Phường Cầu Ông Lãnh, Quận 1', 780000, CAST(N'2024-01-11T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Hồ Văn Anh', N'Số 120, Đường Nguyễn Cư Trinh, Phường Nguyễn Cư Trinh, Quận 1', 600000, CAST(N'2024-01-12T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Thị Thu', N'Số 130, Đường Hồ Tùng Mậu, Phường Bến Nghé, Quận 1', 750000, CAST(N'2024-01-13T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Văn Tài', N'Số 140, Đường Bùi Thị Xuân, Phường Phạm Ngũ Lão, Quận 1', 900000, CAST(N'2024-01-14T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Thị Phương', N'Số 150, Đường Nguyễn Trãi, Phường Bến Thành, Quận 1', 670000, CAST(N'2024-01-15T18:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Văn A', N'Số 10, Đường Nguyễn Văn B, Phường Tân Định, Quận 1', 700000, CAST(N'2024-03-01T08:30:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thị B', N'Số 20, Đường Lê Lai, Phường Bến Thành, Quận 1', 500000, CAST(N'2024-03-02T09:45:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn C', N'Số 30, Đường Phan Văn Hân, Phường 17, Quận Bình Thạnh', 950000, CAST(N'2024-03-03T10:20:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Thị D', N'Số 40, Đường Lê Quang Định, Phường 14, Quận Bình Thạnh', 700000, CAST(N'2024-03-04T11:00:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Phạm Văn E', N'Số 50, Đường Cách Mạng Tháng Tám, Phường 6, Quận 3', 550000, CAST(N'2024-03-05T13:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị F', N'Số 60, Đường Trần Hưng Đạo, Phường Phạm Ngũ Lão, Quận 1', 725000, CAST(N'2024-03-06T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn G', N'Số 70, Đường Hoàng Sa, Phường Tân Định, Quận 1', 700000, CAST(N'2024-03-07T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh I', N'Số 80, Đường Phạm Viết Chánh, Phường 19, Quận Bình Thạnh', 400000, CAST(N'2024-03-08T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị F', N'Số 90, Đường Đề Thám, Phường Phạm Ngũ Lão, Quận 1', 450000, CAST(N'2024-03-09T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn G', N'Số 100, Đường Lê Thánh Tôn, Phường Bến Nghé, Quận 1', 470000, CAST(N'2024-03-10T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh I', N'Số 110, Đường Võ Văn Kiệt, Phường Cầu Ông Lãnh, Quận 1', 480000, CAST(N'2024-03-11T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị F', N'Số 120, Đường Nguyễn Cư Trinh, Phường Nguyễn Cư Trinh, Quận 1', 650000, CAST(N'2024-03-12T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn G', N'Số 130, Đường Hồ Tùng Mậu, Phường Bến Nghé, Quận 1', 700000, CAST(N'2024-03-13T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh I', N'Số 140, Đường Bùi Thị Xuân, Phường Phạm Ngũ Lão, Quận 1', 850000, CAST(N'2024-03-14T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Văn H', N'Số 150, Đường Nguyễn Trãi, Phường Bến Thành, Quận 1', 670000, CAST(N'2024-03-15T18:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thị Lan', N'Số 160, Đường Lê Lai, Phường Bến Thành, Quận 1', 690000, CAST(N'2024-03-16T09:45:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Hoàng Văn Phúc', N'Số 170, Đường Phan Văn Hân, Phường 17, Quận Bình Thạnh', 950000, CAST(N'2024-03-17T10:20:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Thị Thu', N'Số 180, Đường Lê Quang Định, Phường 14, Quận Bình Thạnh', 550000, CAST(N'2024-03-18T11:00:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Phạm Thị Hương', N'Số 190, Đường Cách Mạng Tháng Tám, Phường 6, Quận 3', 500000, CAST(N'2024-03-19T13:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Văn Tùng', N'Số 200, Đường Trần Hưng Đạo, Phường Phạm Ngũ Lão, Quận 1', 625000, CAST(N'2024-03-20T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn X', N'Số 10, Đường Nguyễn Văn B, Phường Tân Định, Quận 1', 800000, CAST(N'2024-02-15T08:30:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thị Y', N'Số 20, Đường Lê Lai, Phường Bến Thành, Quận 1', 600000, CAST(N'2024-02-16T09:45:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Văn Z', N'Số 30, Đường Phan Văn Hân, Phường 17, Quận Bình Thạnh', 950000, CAST(N'2024-02-17T10:20:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Phạm Thị W', N'Số 40, Đường Lê Quang Định, Phường 14, Quận Bình Thạnh', 750000, CAST(N'2024-02-18T11:00:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Văn Q', N'Số 50, Đường Cách Mạng Tháng Tám, Phường 6, Quận 3', 800000, CAST(N'2024-02-19T13:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thị P', N'Số 60, Đường Trần Hưng Đạo, Phường Phạm Ngũ Lão, Quận 1', 725000, CAST(N'2024-02-20T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn O', N'Số 70, Đường Hoàng Sa, Phường Tân Định, Quận 1', 720000, CAST(N'2024-02-21T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh N', N'Số 80, Đường Phạm Viết Chánh, Phường 19, Quận Bình Thạnh', 720000, CAST(N'2024-02-22T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị M', N'Số 90, Đường Đề Thám, Phường Phạm Ngũ Lão, Quận 1', 770000, CAST(N'2024-02-23T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn L', N'Số 100, Đường Lê Thánh Tôn, Phường Bến Nghé, Quận 1', 690000, CAST(N'2024-02-24T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh K', N'Số 110, Đường Võ Văn Kiệt, Phường Cầu Ông Lãnh, Quận 1', 680000, CAST(N'2024-02-25T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Huỳnh Thị J', N'Số 120, Đường Nguyễn Cư Trinh, Phường Nguyễn Cư Trinh, Quận 1', 630000, CAST(N'2024-02-26T15:15:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn I', N'Số 130, Đường Hồ Tùng Mậu, Phường Bến Nghé, Quận 1', 750000, CAST(N'2024-02-27T17:45:00.000' AS DateTime), 1)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Nguyễn Thanh H', N'Số 140, Đường Bùi Thị Xuân, Phường Phạm Ngũ Lão, Quận 1', 900000, CAST(N'2024-02-28T20:00:00.000' AS DateTime), 3)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Lê Văn G', N'Số 150, Đường Nguyễn Trãi, Phường Bến Thành, Quận 1', 670000, CAST(N'2024-02-29T18:30:00.000' AS DateTime), 2)
INSERT [dbo].[Order] ([Id], [CustomerName], [ShippingAddress], [TotalPrice], [CreatedAt], [Discount_Id]) VALUES ( N'Trần Văn X', N'Số 10, Đường Nguyễn Văn B, Phường Tân Định, Quận 1', 800000, CAST(N'2024-02-29T08:30:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (1, 20, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (1, 35, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (2, 21, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (2, 31, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (3, 9, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (3, 30, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (4, 1, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (4, 8, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (5, 11, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (5, 12, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (6, 10, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (6, 12, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (7, 2, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (7, 30, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (8, 2, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (8, 12, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (9, 12, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (9, 33, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (10, 6, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (10, 22, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (11, 1, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (11, 9, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (12, 11, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (12, 29, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (13, 23, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (13, 30, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (14, 10, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (14, 18, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (15, 10, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (15, 29, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (16, 7, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (16, 27, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (17, 3, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (17, 29, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (18, 11, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (18, 21, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (19, 18, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (19, 19, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (20, 7, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (20, 18, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (21, 20, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (21, 26, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (22, 13, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (22, 23, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (23, 14, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (23, 26, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (24, 5, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (24, 26, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (25, 13, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (25, 15, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (26, 21, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (26, 31, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (27, 4, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (27, 24, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (28, 21, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (28, 23, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (29, 11, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (29, 21, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (30, 1, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (30, 21, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (31, 17, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (31, 32, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (32, 10, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (32, 27, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (33, 12, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (33, 27, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (34, 6, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (34, 34, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (35, 10, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (35, 30, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (36, 15, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (36, 31, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (37, 8, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (37, 28, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (38, 9, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (38, 14, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (39, 6, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (39, 33, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (40, 5, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (40, 12, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (41, 4, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (41, 14, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (42, 5, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (42, 20, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (43, 17, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (43, 21, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (44, 2, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (44, 28, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (45, 4, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (45, 26, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (46, 1, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (46, 12, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (47, 1, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (47, 4, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (48, 25, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (48, 29, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (49, 7, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (49, 19, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (50, 11, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (50, 21, 2)
GO
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (51, 1, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (51, 28, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (52, 20, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (52, 33, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (53, 11, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (53, 19, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (54, 19, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (54, 22, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (55, 2, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (55, 8, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (56, 14, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (56, 28, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (57, 23, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (57, 29, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (58, 10, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (58, 30, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (59, 23, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (59, 29, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (60, 11, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (60, 12, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (61, 11, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (61, 30, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (62, 15, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (62, 34, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (63, 13, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (63, 29, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (64, 14, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (64, 22, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (65, 3, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (65, 5, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (66, 10, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (66, 33, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (67, 7, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (67, 15, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (68, 2, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (68, 26, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (69, 26, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (69, 35, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (70, 17, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (70, 18, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (71, 10, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (71, 35, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (72, 19, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (72, 31, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (73, 16, 2)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (73, 21, 3)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (74, 2, 1)
INSERT [dbo].[Order_Book] ([Order_Id], [Book_Id], [NumOfBook]) VALUES (74, 16, 2)
GO