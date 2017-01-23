		var Valve = fabric.util.createClass(fabric.Object, {

			type: 'Valve',
			
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || true);
				this.set('subType', options.subType || '1');
				this.set('innerLineColor', options.innerLineColor || '#0000ff');
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				 
				var x=-this.width/2,y=-this.height/2;
				var str='';
				
				var lineargradient = ctx.createLinearGradient(0, -this.diameter/2, 0,this.diameter/2);
				lineargradient.addColorStop(0, this.fill);
				lineargradient.addColorStop(0.5, 'white');
				// lineargradient.addColorStop(0.75, 'white');
				lineargradient.addColorStop(1, this.fill);

				if (this.subType==='1'){
					ctx.save();
					// ctx.translate(0,0);
					// ctx.scale(0.4,0.4);
					// ctx.rotate(Math.PI/2);
					
					ctx.fillStyle = '#333';
					ctx.strokeStyle = "black";					
					ctx.lineWidth = 2;
					// ctx.lineCap = "round";

					ctx.save();
					// ctx.font = '15px Helvetica';
					ctx.rotate(90*Math.PI/180);
					for (var i=0;i<11;i++){
						ctx.beginPath();
						ctx.rotate(Math.PI*30/180);
						ctx.moveTo(radius,0);
						ctx.lineTo(radius*1.2,0);						
						// ctx.fillText(i, 80, 0);
						ctx.stroke();
					}
					ctx.restore();
					
					ctx.save();
					ctx.rotate(120*Math.PI/180);
					ctx.lineWidth = 1;
					for (i=0;i<50;i++){
					if (i%5!=0) {
					  ctx.beginPath();
					  ctx.moveTo(radius*1.05,0);
					  ctx.lineTo(radius*1.2,0);
					  ctx.stroke();
					}
					ctx.rotate(Math.PI/30);
					}
					ctx.restore();
					
					ctx.save();
					ctx.rotate(120*Math.PI/180);
					ctx.strokeStyle = "#D40000";
					ctx.lineWidth =2;
					ctx.beginPath();
					ctx.rotate(this.myValue*Math.PI/60);
					ctx.moveTo(-radius*0.1,0);
					ctx.lineTo(radius*0.9,0);
					ctx.stroke();
					ctx.arc(0,0,5,0,Math.PI*2,true);
					ctx.fillStyle = "#D40000";
					ctx.fill();
					ctx.restore();
					
					ctx.font = radius*0.3+'px Helvetica';
					ctx.fillText(this.myValue, -radius*0.2, radius*0.5);
					
					
					ctx.font = radius*0.15+'px Helvetica';
					for (var i=0;i<11;i++){
						if(i<5)
							ctx.fillText(i*10, -(radius*0.9)*Math.cos((60-i*30)*Math.PI/180)-radius*0.05,(radius*0.8)*Math.sin((60-i*30)*Math.PI/180)+radius*0.05);
						else if(i===5)
							ctx.fillText(i*10, -(radius*0.9)*Math.cos((60-i*30)*Math.PI/180)-radius*0.1,(radius*0.8)*Math.sin((60-i*30)*Math.PI/180));
						else
							ctx.fillText(i*10, -(radius*0.9)*Math.cos((60-i*30)*Math.PI/180)-radius*0.1,(radius*0.8)*Math.sin((60-i*30)*Math.PI/180)+radius*0.05);
						console.log()
					}
					
					ctx.strokeStyle = "#D40000";	
					ctx.beginPath();
					ctx.arc(0,0,radius*1.2,120*Math.PI/180,420*Math.PI/180,false);
					ctx.stroke();
					
					ctx.restore();
					
					
				}else if(this.subType==='2'){
					
					
				}else if(this.subType==='3'){
					
				}else if(this.subType==='4'){
					
					
				}
				this.setCoords();
			}
		});
		
		ValveProperty=['subType','myValue','innerLineColor'];
		Valve.prototype.property=ValveProperty;	
		
		var ToolValve = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var valve = new Valve({
					subType:'1',myValue:true,
					left: pointer.x, top: pointer.y ,
					fill:'#000000',
					stroke: '#000000',	strokeWidth:1 ,
					width:200,	height:200					
				});
			this.addNewObject(valve);
			}
			
		});