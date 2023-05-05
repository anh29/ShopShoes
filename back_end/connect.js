let sql = require("mssql/msnodesqlv8");

//các thông tin kết nối csdl
let config = {
    server: "MSI\\SQLEXPRESS01",
    user: "sa",
    password: "123456",
    database: "ShopGiayDemo",
    driver: "msnodesqlv8",
    option: {
        trustedConnection: true
    }
};

const cnn = new sql.ConnectionPool(config).connect().then(pool => {
    return pool;
});

//xuất ra dưới dạng module gồm 2 thuộc tính là cnn và sql
module.exports = {
    cnn: cnn,
    sql: sql
}
