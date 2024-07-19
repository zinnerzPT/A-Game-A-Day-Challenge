extends Node

export var circle_scene : PackedScene
export var cross_scene : PackedScene

var player : int
var winner : int
var moves : int

var temp_marker
var player_panel_pos : Vector2

var grid_data : Array

var grid_pos : Vector2

var board_size : int
var cell_size : int

var board_position_offset: Vector2

var row_sum : int
var column_sum : int
var diagonal1_sum : int
var diagonal2_sum : int

# Called when the node enters the scene tree for the first time.
func _ready():
	board_size = $Board.texture.get_width()*$Board.transform.get_scale().x
	board_position_offset = $Board.position - $Board.texture.get_size()*$Board.transform.get_scale()/2
	
	# divide board size by 3 to get the size of individual cell
	cell_size = board_size/3
	
	# get coordinates of small panel on right side
	player_panel_pos = $PlayerPanel.rect_position
	
	new_game()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.pressed:
			# check if mouse is on the board
			if (event.position.x>board_position_offset.x and event.position.y>board_position_offset.y 
			and event.position.x<board_size+board_position_offset.x 
			and event.position.y<board_size+board_position_offset.y):
				# Convert mouse position to grid location
				var relative_position = event.position-board_position_offset
				grid_pos = (relative_position/cell_size).floor()
				if grid_data[grid_pos.y][grid_pos.x] == 0:
					moves += 1
					grid_data[grid_pos.y][grid_pos.x] = player
					# place that player's marker
					create_marker(player,grid_pos*cell_size + board_position_offset + Vector2(cell_size/2,cell_size/2))
					if check_win() != 0:
						get_tree().paused=true
						$GameOverMenu.show()
						if winner == 1:
							$GameOverMenu.get_node("ResultLabel").text = "Circles Win!"
						else:
							$GameOverMenu.get_node("ResultLabel").text = "Crosses Win!"
					#check if the board has been filled
					elif moves == 9:
						get_tree().paused=true
						$GameOverMenu.show()
						$GameOverMenu.get_node("ResultLabel").text = "It's a Draw!"
					player *= -1
					# update the panel marker
					temp_marker.queue_free()
					create_marker(player, player_panel_pos + Vector2(cell_size/2, cell_size/2), true)
					print(grid_data)

func new_game():
	player = 1
	winner = 0
	moves = 0
	grid_data = [
		[0, 0, 0],
		[0, 0, 0],
		[0, 0, 0]
		]
	row_sum = 0
	column_sum = 0
	diagonal1_sum = 0
	diagonal2_sum = 0
	#clear existing markers
	get_tree().call_group("circles", "queue_free")
	get_tree().call_group("crosses", "queue_free")
	#create a marker to show starting player's turn
	create_marker(player, player_panel_pos + Vector2(cell_size/2, cell_size/2), true)
	$GameOverMenu.hide()
	get_tree().paused = false

func create_marker(player, position, temp = false):
	#create a marker node and add it as a child
	if player==1:
		var circle = circle_scene.instance()
		circle.position = position
		add_child(circle)
		if temp: temp_marker = circle
	elif player == -1:
		var cross = cross_scene.instance()
		cross.position = position
		add_child(cross)
		if temp: temp_marker = cross

func check_win():
	#add up the marrkers in each row, column and diagonal
	for i in len(grid_data):
		row_sum = grid_data [i][0] + grid_data [i][1] + grid_data [i][2]
		column_sum = grid_data [0][i] + grid_data [1][i] + grid_data [2][i]
		if row_sum == 3 or column_sum == 3:
			winner = 1
		elif row_sum == -3 or column_sum == -3:
			winner = -1
	
	diagonal1_sum = grid_data [0][0] + grid_data [1][1] + grid_data [2][2]
	diagonal2_sum = grid_data [0][2] + grid_data [1][1] + grid_data [2][0]
	
	if diagonal1_sum == 3 or diagonal2_sum == 3:
		winner = 1
	elif diagonal1_sum == -3 or diagonal2_sum == -3:
		winner = -1
	
	return winner


func _on_GameOverMenu_restart():
	new_game()
