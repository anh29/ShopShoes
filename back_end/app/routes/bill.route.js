module.exports = function (app) {
    let billController = require('../controllers/bill.controller');
    // app.get('/bill', billController.getList);
    
    // app.get('/bill/:id', billController.getById);    
    
    //nhận dữ liệu từ client gửi lên thông qua phương thức post (add)
    app.post('/bill', billController.addNew);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức put (update)
    // app.put('/customer', productController.update);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức delete
    // app.delete('/customer/:id', productController.delete);
};