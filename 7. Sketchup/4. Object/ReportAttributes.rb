# Sketchup 표준 API 를 사용하기 위해 sketchup.rb 파일을 포함시킨다
require 'sketchup.rb'
Sketchup.send_action "showRubyPanel:"

# Plug-in이 실행될 때 메뉴를 추가.
UI.menu("PlugIns").add_item("Report attributes") { 
	report	
}

def report
		index = 0
		Sketchup.active_model.entities.each { | entity |  
			attrdict = entity.attribute_dictionaries['materials']
			if attrdict == nil 
				UI.messagebox("There are no attributes")
			else
				attrdict.each { | key, value |   UI.messagebox key.to_s + '=' + value.to_s }
			end
			index = index + 1
			if index > 3
				break 
			end
		}
end