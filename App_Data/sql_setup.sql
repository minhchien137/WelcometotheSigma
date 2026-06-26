-- Chạy trong database: zkbiotime
-- =========================================================
-- Bảng khách VIP (thêm tên trực tiếp vào đây)
-- =========================================================
CREATE TABLE VipGuest (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    GuestName     NVARCHAR(200) NOT NULL,
    ImageFileName NVARCHAR(500) NULL,          -- tên file ảnh, ví dụ: Nguyen_Van_A.jpg
    CreatedAt     DATETIME2     DEFAULT GETDATE()
);

-- =========================================================
-- Bảng lịch hiển thị (admin quản lý qua giao diện web)
-- =========================================================
CREATE TABLE VipDisplaySchedule (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    GuestId   INT      NOT NULL,
    StartTime TIME(0)  NOT NULL,               -- ví dụ: 08:00
    EndTime   TIME(0)  NOT NULL,               -- ví dụ: 09:00
    IsActive  BIT      NOT NULL DEFAULT 1,
    CreatedAt DATETIME2         DEFAULT GETDATE(),
    CONSTRAINT FK_VipSchedule_Guest FOREIGN KEY (GuestId) REFERENCES VipGuest(Id)
);

-- =========================================================
-- Ví dụ: thêm khách VIP thủ công
-- =========================================================
-- INSERT INTO VipGuest (GuestName) VALUES (N'Nguyễn Văn A');
-- INSERT INTO VipGuest (GuestName) VALUES (N'Trần Thị B');
