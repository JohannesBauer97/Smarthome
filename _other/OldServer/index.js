var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);
net = require('net');
var clients = [];

app.use(express.static("public"));

app.get('/', function(req, res){
  res.sendFile(__dirname + '/public/index.html');
});

io.on('connection', function(socket){
  console.log('Website connected');

  socket.on('disconnect', function(){
    console.log('Website disconnected');
  });

  socket.on('cmd', function(msg){
    console.log(msg);
    clients[0].write(msg,"ascii");
  });

  socket.on('colorpicker', function(msg){
    console.log(msg);
    clients[0].write(msg,"ascii");
  });

});

http.listen(3000, function(){
  console.log('listening on *:3000');
});


net.createServer(function (socket) {
  socket.name = socket.remoteAddress + ":" + socket.remotePort 
  clients.push(socket);
  console.log("Client connected");

  socket.setEncoding("ascii")
  socket.on('data', function (data) {
    console.log(data);
  });

  socket.on('end', function () {
    clients.splice(clients.indexOf(socket), 1);
  });

}).listen(5555);

console.log("Server running in port 5555");