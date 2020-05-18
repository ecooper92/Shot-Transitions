# Shot Transitions Detection
Shot Transitions in a windows application that detects and visualizes changes in scene within a video. It leverages several different algorithms to detect both abrupt scene transitions and gradual fade-in/out.

## Example
The following screenshot shows the execution of the application once it has processed a video.

The top chart shows any deviations from the average state of the scene. Objects moving through the scene will generate deviation in this line, but abrupt scene changes should generate a large spike.

The bottom chart shows the average intensity of the pixels in the scene. Fade-in/out can be detected by analysing intensity rises and falls within the scene.

The red ticks indicate where the algorithms believe the shots start and stop. 

![screenshot](https://user-images.githubusercontent.com/22899644/82269525-9fb65880-9937-11ea-8aac-570fc721c8e3.png)
