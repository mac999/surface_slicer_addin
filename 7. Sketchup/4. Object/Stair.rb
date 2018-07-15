# Sketchup 표준 API 를 사용하기 위해 sketchup.rb 파일을 포함시킨다
require 'sketchup.rb'
Sketchup.send_action "showRubyPanel:"
# Plug-in이 실행될 때 메뉴를 추가.
UI.menu("PlugIns").add_item("Draw spiral stairs") { 
draw_stairs
}

def draw_stairs  
# 도형을 관리하는 모델 객체를 획득 
model = Sketchup.active_model  
entities = model.entities  
# 계단을 포현하는 포인트의 좌표값을 설정
pt1 = [0, 0, 0]  
pt2 = [9, 0, 0]  
pt3 = [9, 9, 0]  
pt4 = [0, 9, 0]    
# 포인트들로 구성된 면 하나를 생성
new_face = entities.add_face pt1, pt2, pt3, pt4
end

