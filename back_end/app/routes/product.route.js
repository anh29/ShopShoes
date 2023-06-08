module.exports = function (app) {
    let productController = require('../controllers/product.controller');
    app.get('/product', productController.getList);
    
    app.get('/product/:id', productController.getById);    
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức post (add)
    // app.post('/product', productController.addNew);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức put (update)
    // app.put('/product', productController.update);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức delete
    // app.delete('/product/:id', productController.delete);
};