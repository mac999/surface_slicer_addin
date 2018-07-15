# Sketchup 표준 API 를 사용하기 위해 sketchup.rb 파일을 포함시킨다
require 'sketchup.rb'
Sketchup.send_action "showRubyPanel:"
# Plug-in이 실행될 때 메뉴를 추가.

UI.menu("PlugIns").add_item("Draw spiral stairs group and attributes") { 
	UI.messagebox("I'm about to draw spiral stairs!")
	draw_stairs 
}


def draw_stairs
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

  
# 계단 면 생성
		pt1 = Geom::Point3d.new radius, 0, 0
		pt1 = [0, 0, 0]
		pt2 = Geom::Point3d.new radius, 0, 0
		pt2 = [0, 10, 0]
		pt3 = Geom::Point3d.new radius, 0, 0
		pt3 = [10, 10, 0]
		pt4 = Geom::Point3d.new radius, 0, 0
		pt4 = [10, 0, 0]
	

		group = entities.add_group
		new_face = group.entities.add_face pt1, pt2, pt3, pt4  
			
new_face.material = Sketchup::Color.new(step % 256, 100, 0)	# 색상 설정. 
# 색상범위가 0-255이므로 나머지 연산자 %를 사용함.
		new_face.pushpull 1 true		# 계단 두께 생성

		# 속성 설정
		group.set_attribute "stair_parameter", "radius", radius
		group.set_attribute "stair_parameter", "width", width
		group.set_attribute "stair_parameter", "thickness", thickness
		group.set_attribute "stair_parameter", "angle", angle
		group.set_attribute "material", "finish", "wood-fin-n0425"
		group.set_attribute "material", "core", "steel-n1005"
		group.set_attribute "material", "modulus", ""
end