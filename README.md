Brain Biopsy Simulation & Trajectory Visualization

Overview:
This Unity application provides an interactive 3D simulation environment for planning and visualizing brain biopsies, including:
- Interactive slice-based biopsy targeting with entry and target points.
- ROS integration to publish coordinates for robotic execution.
- Trajectory playback for robotic arms based on JSON or ROS trajectory messages.
- 3D navigation and UI management for immersive visualization.
The project integrates medical imaging (DICOM), robotics, and 3D visualization in Unity.

Features:
1. Brain Slices & Biopsy Planning
- Slice Display: Axial, sagittal, and coronal slices of a 3D brain volume.
- Interactive Targeting: Right-click on each slice to select a biopsy target.
- Entry Point Selection: After selecting the target, pick an entry point on the 3D brain surface.
- Reset Options: Buttons allow resetting either the target or entry point.
- ROS Publishing: Selected points are published to ROS topics (/target_point and /entry_point) via CoordinatesPublisher.

2. Volume Loading
- DICOM Loader (Loader1): Loads 3D brain volumes from DICOM files.
- Slice Generation: Generates axial, sagittal, and coronal Texture2D slices for display.
- Flexible Filename Parsing: Supports varied naming conventions for DICOM slices.
  
3. Slice Visualization
- SliceDisplay: Handles positioning, rotation, and scaling of slices in 3D space.
- Custom Shader (SliceRotateFinal): Correctly orients slice textures with rotation and mirroring.
  
4. User Interface
- Biopsy Canvas: Dynamically generated buttons for resetting points.
- UI Toggle (UIToggle): Show or hide the UI to focus on 3D visualization.

5. Camera Controls
- WASDOrbitCamera: Orbit around the target using WASD or arrow keys.
- Mouse Zoom: Scroll wheel zooms in and out while maintaining target focus.

6. Trajectory Playback
- SimpleTrajectoryPlayer: Plays pre-recorded robot trajectories from JSON files.
- TrajectoryReceiver: Receives and stores ROS joint trajectory messages for real-time robotic execution.
- Supports both rotating and sliding joints for realistic arm simulation.

Scripts Overview:
1) BiopsyClickManager:	Handles slice clicks, calculates biopsy target, entry point selection, and ROS publishing.
2) SliceDisplay:	Displays a single slice in 3D, adjusting position, rotation, and scale according to the brain model.
3) SliceManager: Updates slice displays based on slider input for axial, sagittal, and coronal planes.
4) CoordinatesPublisher:	Publishes biopsy target and entry points to ROS topics.
5) Loader1:	Loads DICOM files, converts them into 3D voxel data, and generates 2D slice textures.
6) TrajectoryReceiver:	Subscribes to ROS trajectories and stores joint waypoints.
7) SimpleTrajectoryPlayer:	Plays trajectory from a JSON file, applying joint rotations and positions to the robot model. (An example is available in RobotAnimation scene with a pre-recorded trajectory)
8) UIToggle:	Toggles visibility of main and biopsy UI canvases.
9) WASDOrbitCamera:	Provides orbiting and zooming camera controls around a target object.
    
Controls:
Action	                       Input
Orbit Camera	                 WASD / Arrow Keys
Zoom Camera	Mouse              Scroll Wheel
Select Slice                   Point	Right Click
Reset Target	                 Reset Target Point Button
Reset Entry	                   Reset Entry Point Button
Toggle UI	                     Button linked to UIToggle

Shader:
SliceRotateFinal
- Rotates the slice texture 90° counter-clockwise.
- Applies horizontal and vertical flipping to match anatomical orientation.
- Used in SliceDisplay for correct visual alignment.
