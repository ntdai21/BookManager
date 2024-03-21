CREATE TABLE `Book` (
	`Id` INT NOT NULL AUTO_INCREMENT,
	`Name` NVARCHAR(255),
	`Price` FLOAT,
	`NumOfPage` INT,
	`PublishingCompany` NVARCHAR(255),
	`Author` NVARCHAR(255),
	`Category_Id` INT NOT NULL,
	`BookCovers` NVARCHAR(255),
	`Description` NVARCHAR(255),
	PRIMARY KEY (`Id`)
);

CREATE TABLE `Category` (
	`Id` INT NOT NULL AUTO_INCREMENT,
	`Name` NVARCHAR(255) NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `Order` (
	`Id` INT NOT NULL AUTO_INCREMENT,
	`CustomerName` NVARCHAR(255),
	`TotalPrice` FLOAT NOT NULL,
	`CreatedAt` DATETIME NOT NULL,
	`Discount_Id` INT,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `Order_Book` (
	`Order_Id` INT NOT NULL,
	`Book_Id` INT NOT NULL,
	`NumOfBook` INT NOT NULL,
	PRIMARY KEY (`Order_Id`,`Book_Id`)
);

CREATE TABLE `Discount` (
	`Id` INT NOT NULL AUTO_INCREMENT,
	`Code` VARCHAR(255) NOT NULL,
	`DiscountPercent` FLOAT NOT NULL,
	`MaxDiscount` FLOAT,
	PRIMARY KEY (`Id`)
);

ALTER TABLE `Book` ADD CONSTRAINT `Book_fk0` FOREIGN KEY (`Category_Id`) REFERENCES `Category`(`Id`);

ALTER TABLE `Order` ADD CONSTRAINT `Order_fk0` FOREIGN KEY (`Discount_Id`) REFERENCES `Discount`(`Id`);

ALTER TABLE `Order_Book` ADD CONSTRAINT `Order_Book_fk0` FOREIGN KEY (`Order_Id`) REFERENCES `Order`(`Id`);

ALTER TABLE `Order_Book` ADD CONSTRAINT `Order_Book_fk1` FOREIGN KEY (`Book_Id`) REFERENCES `Book`(`Id`);



INSERT INTO Discount (Code, DiscountPercent, MaxDiscount)
VALUES
	("HAPPYNEWYEAR",10,50000),
	("OPENING",15,30000),
	("BDAY",20,100000);

INSERT INTO Category (Name)
VALUES
	("Truyện Trinh thám"),
	("Sách Văn học"),
	("Truyện Kinh dị");

INSERT INTO Book (Name, Price, NumOfPage, PublishingCompany, Author, Category_Id, BookCovers, Description)
VALUES
    ("Rừng than khóc", 189000, 488, "NXB Thanh Niên", "Thục Linh", 1,"/Resources/BookCovers/rung-than-khoc.jpg","Description..."),
    ("Fourth Wing", 315000, 824, "NXB Trẻ", "Rebecca Yarros", 2,"/Resources/BookCovers/canh-tu.jpg", "Description..."),
    ("Cái chết hạnh phúc", 98000, 216, "NXB Văn Học", "Albert Camus", 1, "/Resources/BookCovers/cai-chet-hanh-phuc.jpg", "Description..."),
    ("Thần thoại Celt", 175000, 323, "NXB Dân Trí", "Albert Camus", 1, "/Resources/BookCovers/than-thoai-celt.jpg", "Description..."),
    ("Chim ruồi rực cháy", 199000, 464, "NXB Văn học", "Tân Minh", 3, "/Resources/BookCovers/than-thoai-celt.jpg", "Description..."),
    ("Chim ruồi rực cháy", 199000, 464, "NXB Văn học", "Tân Minh", 3, "/Resources/BookCovers/chim-ruoi-ruc-chay.jpg", "Description..."),
    ("Trở lại hiện trường vụ án", 165000, 387, "NXB Văn học", "Mã Định Hàn Trang", 3, "/Resources/BookCovers/tro-lai-hien-truong-vu-an.jpg", "Description..."),
    ("Những cô gái cuối cùng", 199000, 256, "NXB Lao Động", "Riley Sager", 3, "/Resources/BookCovers/nhung-co-gai-cuoi-cung.jpg", "Description..."),
    ("Những hành tinh của Robin", 179000, 425, "NXB Văn học", "Richard Powers", 1, "/Resources/BookCovers/nhung-hanh-tinh-cua-robin.jpg", "Description..."),
    ("Giận", 129000, 225, "NXB Thế Giới", "Thích Nhất Hạnh", 1, "/Resources/BookCovers/gian.jpg", "Description..."),
    ("Hội cố thi nhân", 149000, 275, "NXB Thanh Niên", "Nancy H. Kleinbaum", 2, "/Resources/BookCovers/hoi-co-thi-nhan.jpg", "Description..."),
    ("Dê mặt quỷ", 249000, 556, "NXB Thanh Niên", "Chan-Ho-Kei", 2, "/Resources/BookCovers/de-mat-quy.jpg", "Description..."),
    ("Juliette Và Tiệm Sách Bí Ẩn Ở Paris", 159000, 284, "NXB Thanh Niên", "Christine Féret-Fleury", 1, "/Resources/BookCovers/juliette-va-tiem-sach-bi-an-o-paris.jpg", "Description..."),
    ("Lạc Vào Vùng Ký Ức", 179000, 456, "NXB Thanh Niên", "Sarah Addison Allen", 2, "/Resources/BookCovers/lac-vao-vung-ky-uc.jpg", "Description..."),
    ("Biến Thể Của Cô Đơn", 70000, 156, "NXB Trẻ", "Yang Phan", 2, "/Resources/BookCovers/bien-the-cua-co-don.jpg", "Description..."),
    ("Những Quân Bài Trên Mặt Bàn", 170000, 316, "NXB Trẻ", "Agatha Christie", 2, "/Resources/BookCovers/nhung-quan-bai-tren-mat-ban.jpg", "Description..."),
    ("Đảo chìm", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/dao-chim.jpg", "Description..."),
    ("Sự Kiên Định Của Kẻ Săn Mồi", 170000, 216, "NXB Thanh Niên", "Maxime Chattam", 1, "/Resources/BookCovers/su-kien-dinh-cua-ke-san-moi.jpg", "Description..."),
    ("Nàng Cựu Idol Lớp Tôi Lại Có Hành Động Đáng Ngờ Nữa Rồi - Tập 2", 149000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/nang-cuu-idol-lop-toi-lai-co-hanh-dong-dang-ngo-nua-roi-tap-2.jpg", "Description..."),
    ("Xa Một Chút Hơn Một Triệu Năm Ánh Sáng", 150000, 216, "NXB Hà Nội", "Hideyuki Furuhashi", 1, "/Resources/BookCovers/xa-mot-chut-hon-mot-trieu-nam-anh-sang.jpg", "Description..."),
    ("Ác Duyên - Duyên Khởi", 155000, 316, "NXB Hội Nhà Văn", "Rosie Nguyễn", 1, "/Resources/BookCovers/ac-duyen.jpg", "Description..."),
    ("Chương Cuối Của Mùa Hạ", 120000, 216, "NXB Hội Nhà Văn", "Nam Thanh", 1, "/Resources/BookCovers/chuong-cuoi-cua-mua-ha.jpg", "Description..."),
    ("Phố Vẫn Gió", 168000, 288, "NXB Hội Nhà Văn", "Lê Minh Hà", 1, "/Resources/BookCovers/pho-van-gio.jpg", "Description..."),
    ("Gió Tự Thời Khuất Mặt", 172000, 216, "NXB Hội Nhà Văn", "Lê Minh Hà", 1, "/Resources/BookCovers/gio-tu-thoi-khuat-mat.jpg", "Description..."),
    ("W Hay Là Ký Ức Tuổi Thơ", 110000, 216, "NXB Hội Nhà Văn", "Georges Perec", 1, "/Resources/BookCovers/w-hay-la-ky-uc-tuoi-tho.jpg", "Description..."),
    ("Những Kẻ Ăn Sách", 230000, 216, "NXB Thanh Niên", "Sunyi Dean", 1, "/Resources/BookCovers/nhung-ke-an-sach.jpg", "Description..."),
    ("Nỗi Cô Đơn Của Các Số Nguyên Tố", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/noi-co-don-cua-cac-so-nguyen-to.jpg", "Description..."),
    ("Vụ Án Sôcôla Có Độc", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/vu-an-socola-co-doc.jpg", "Description..."),
    ("Bản Du Ca Cuối Cùng", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/ban-du-ca-cuoi-cung.jpg", "Description..."),
    ("17 Âm 1", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/17-am-1.jpg", "Description..."),
    ("Ngủ Giấc Ngàn Thu", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/ngu-giac-ngan-thu.jpg", "Description..."),
    ("Thế Giới Atlantis", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/the-gioi-atlantis.jpg", "Description..."),
    ("Chitose Trong Chai Ramune - Tập 5", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/chitose-trong-chai-ramune-tap-5.jpg", "Description..."),
    ("Bác Hana", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/bac-hana.jpg", "Description..."),
    ("Xoắn Ốc Vô Hình", 100000, 216, "NXB Hội Nhà Văn", "Trần Đăng Khoa", 1, "/Resources/BookCovers/xoan-oc-vo-hinh.jpg", "Description...")
