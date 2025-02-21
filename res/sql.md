
// uuid 创建和插入

CREATE TABLE IF NOT EXISTS `user` (
    `id` CHAR(36) NOT NULL DEFAULT (UUID()),  -- 使用UUID()函数生成唯一ID
    `name` VARCHAR(10) NOT NULL,  -- 姓名，最多10个字符
    `loginName` VARCHAR(10) NOT NULL,  -- 登录用户名，最多10个字符
    `phone` VARCHAR(20) NOT NULL,  -- 手机号，支持国际格式
    `userNumber` INT NOT NULL,  -- 员工编号，8位整数
    `deptId` VARCHAR(50) NOT NULL,  -- 部门ID
    `userExpiryDate` DATE NOT NULL,  -- 用户有效期止（格式：yyyy-MM-dd）
    `status` TINYINT(1) DEFAULT 1,  -- 用户状态，1为启用（默认为启用），0为停用
    `level` TINYINT(1) DEFAULT 1,  -- 级别，1：一级，2：二级（默认为一级）
    `money` DOUBLE DEFAULT NULL,  -- 补助，可为空
    `age` INT(3) DEFAULT NULL,  -- 年龄，正整数，可为空
    `notes` VARCHAR(100) DEFAULT NULL,  -- 备注，最多100个字符
		`isDelete` TINYINT(1) DEFAULT 0,  -- 是否删除，1删除，0未删除
    PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;



INSERT INTO `user` 
(`name`, `loginName`, `phone`, `userNumber`, `deptId`, `userExpiryDate`, `status`, `level`, `money`, `age`, `notes`, `isDelete`)
VALUES
('张三', 'zhangsan', '13812345678', 10000001, 'dept001', '2025-12-31', 1, 1, 1500.00, 28, '备注信息1', 0),
('李四', 'lisi', '13812345679', 10000002, 'dept002', '2025-12-31', 1, 2, 2000.00, 30, '备注信息2', 0),
('王五', 'wangwu', '13812345680', 10000003, 'dept003', '2025-12-31', 1, 1, 1800.00, 32, '备注信息3', 0),
('赵六', 'zhaoliu', '13812345681', 10000004, 'dept004', '2025-12-31', 1, 2, 2200.00, 25, '备注信息4', 0),
('孙七', 'sunqi', '13812345682', 10000005, 'dept001', '2025-12-31', 0, 1, 1600.00, 27, '备注信息5', 1),
('周八', 'zhouba', '13812345683', 10000006, 'dept002', '2025-12-31', 1, 2, 1900.00, 29, '备注信息6', 0),
('吴九', 'wujiu', '13812345684', 10000007, 'dept003', '2025-12-31', 1, 1, 1700.00, 33, '备注信息7', 1),
('郑十', 'zhengshi', '13812345685', 10000008, 'dept004', '2025-12-31', 0, 1, 2100.00, 31, '备注信息8', 0),
('钱十一', 'qianshiyi', '13812345686', 10000009, 'dept001', '2025-12-31', 1, 2, 2300.00, 26, '备注信息9', 1),
('张十二', 'zhangshier', '13812345687', 10000010, 'dept002', '2025-12-31', 1, 1, 1600.00, 35, '备注信息10', 0);


INSERT INTO `user` 
(`name`, `loginName`, `phone`, `userNumber`, `deptId`, `userExpiryDate`, `status`, `level`, `money`, `age`, `notes`, `isDelete`)
VALUES
('张十二', 'zhangshier', '13812345687', 10000010, 'dept002', '2025-12-31', 1, 1, 1600.00, 35, '备注信息10', 0);



SELECT * FROM User WHERE Id="d788f489-ef2a-11ef-a968-ecd68acb276a"



DELETE FROM User WHERE Id="d788f489-ef2a-11ef-a968-ecd68acb276a"




