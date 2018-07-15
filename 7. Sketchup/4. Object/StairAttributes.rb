# Sketchup 표준 API 를 사용하기 위해 sketchup.rb 파일을 포함시킨다
require 'sketchup.rb'
Sketchup.send_action "showRubyPanel:"
# Plug-in이 실행될 때 메뉴를 추가.

UI.menu("PlugIns").add_item("Draw stair with attributes") { 
	UI.messagebox("I'm about to draw spiral stairs!")
	draw_stairs
	
}

def draw_stairs  
	# 나선형 계단 생성을 위한 파라메터를 생성
	radius = 200
	angle_interval = 3.14159 / 30.0
	stairs = 255  
	rise = 8  
	run = 12  
	width = 100  
	thickness = 3
	# 도형을 관리하는 모델 객체를 획득
	model = Sketchup.active_model  
	entities = model.entities  

	# 같은 코드를 반복 수행 위해 for loop 문법 사용. 1에서 stairs 횟수까지 반복
	for step in 1..stairs        
		# 계단의 각 코너를 계산 
		x1 = 0    
		y1 = 0    
		x2 = width    
		y2 = run 
		z = rise * step        

		# 나선형 계단을 만들기 위해 이동 * 회전 변환행렬을 만듬
		point = Geom::Point3d.new radius, 0, 0
		tMove = Geom::Transformation.new point
		point = [0, 0, 0]
		vector = [0, 0, 1]
		angle = angle_interval * step
		tRotation = Geom::Transformation.rotation point, vector, angle
		transformation = tRotation * tMove

		# 계산된 좌표값을 이용해 계산을 이루는 각 포인트를 생성
		pt1 = transformation * [x1, y1, z]    
		pt2 = transformation * [x2, y1, z]    
		pt3 = transformation * [x2, y2, z]    
		pt4 = transformation * [x1, y2, z]    

		# 계단 면 생성
		group = entities.add_group
		new_face = group.entities.add_face pt1, pt2, pt3, pt4  			
		new_face.material = Sketchup::Color.new(step % 256, 100, 0)	# 색상 설정. 
							# 색상범위가 0-255이므로 나머지 연산자 %를 사용함.
		new_face.pushpull thickness		# 계단 두께 생성
		
		create_if_nil = true 
		attrdict = group.attribute_dictionary "materials", create_if_nil 
		attrdict["no"] = step 
		attrdict["material"] = "wood-steel"
		attrdict["company"] = "woodproduct.com"
	end
end
