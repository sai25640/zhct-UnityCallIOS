//
//  UnityTest.m
//  Unity-iPhone
//
//  Created by Leon on 2018/7/27.
//

#import <Foundation/Foundation.h>
#import "UnityTest.h"
#import <MobileCoreServices/MobileCoreServices.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <AVKit/AVKit.h>
@interface UnityTest ()
@property(nonatomic,strong)AVPlayer * player;
@property(nonatomic,strong)AVPlayerLayer * playerLayer;
@property(nonatomic,assign)CGFloat x;
@property(nonatomic,assign)CGFloat y;
@property(nonatomic,assign)CGFloat w;
@property(nonatomic,assign)CGFloat h;
@end
@implementation UnityTest

//OC的单例模式
+(instancetype)shareManager{
    static UnityTest *controller = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        controller = [[self alloc] init];
    });
    return controller;
}

//打开相机或者相册
-(void)OpenTarget:(UIImagePickerControllerSourceType) type{
    UIImagePickerController *picker;
    picker = [[UIImagePickerController alloc]init];
    picker.delegate = self;
    picker.allowsEditing = NO;
    picker.sourceType = type;
    picker.mediaTypes = @[@"public.movie"];
    NSLog(@"%@",picker.mediaTypes);
    
    [self presentViewController: picker animated:YES completion:nil];
}

//选择图片或者视频
-(void)imagePickerController:(UIImagePickerController *)pic didFinishPickingMediaWithInfo:(NSDictionary *)
info{
    [pic dismissViewControllerAnimated:YES completion:nil];
    NSString *mediaType=[info objectForKey:UIImagePickerControllerMediaType];
    if ([mediaType isEqualToString:@"public.image"]) {//如果是拍照
        UIImage *image = info[UIImagePickerControllerOriginalImage];
        
        NSString *imagePath =[self GetSavePath:@"temp.jpg"];
        [self SaveFileToDoc: image path:imagePath];

    }else if([mediaType isEqualToString:@"public.movie"]){
        NSURL * url = [info objectForKey:UIImagePickerControllerMediaURL];
    
         //根据URL创建播放曲目
        AVPlayerItem * item = [AVPlayerItem playerItemWithURL:url];

        //获取视频时长
        CGFloat videoTime = item.asset.duration.value/item.asset.duration.timescale;
        NSString *stringFloat = [NSString stringWithFormat:@"%f",videoTime];
        //将曲目添加到播放器
        self.player = [AVPlayer playerWithPlayerItem:item];

        //创建一个视频播放图层
        self.playerLayer = [AVPlayerLayer playerLayerWithPlayer:self.player];

        //设置视频播放图层的frame(宽高比最好是16：9)
        CGFloat screenWidth = [UIScreen mainScreen].bounds.size.width;
        self.playerLayer.frame = CGRectMake(self.x, self.y, self.w, self.h);
        //self.playerLayer.frame = CGRectMake(0, 0, 200 , 100);

        //将视频播放图层添加到父控件图层
        [self.view.layer addSublayer:self.playerLayer];


        [self.player play];

        NSData * data = [NSData dataWithContentsOfURL:url];
        //NSLog(@"%@",data);  
        NSString *videoPath =[self GetSavePath:@"temp.mp4"];
        NSString *videoStr = [data base64Encoding];
        UnitySendMessage("Main Camera", "IOSBack", videoStr.UTF8String);
        UnitySendMessage("Main Camera", "IOSBack2",stringFloat.UTF8String);
    }
    
}

//删除IOS的视频层
- (void)deletePlayer{
    [self.playerLayer removeFromSuperlayer];
}

//视频播放完毕后的回调
- (void)playerItemDidPlayToEnd:(NSNotification *)notification{ 
    [self.player play]; 
}


-(NSString *)GetSavePath:(NSString *)filename{
    NSArray *pathArray = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *docPath =[pathArray objectAtIndex:0];
    return [docPath stringByAppendingPathComponent:filename];
}

-(void)SaveFileToDoc:(UIImage *)image path:(NSString *)path{
    NSData *data;
    
    if (UIImagePNGRepresentation(image)==nil) {
        data = UIImageJPEGRepresentation(image, 1);
    }else{
        data = UIImagePNGRepresentation(image);
    }
    
    NSString *imageStr = [data base64Encoding];
    UnitySendMessage("Main Camera", "IOSBack", imageStr.UTF8String);
    //[data writeToFile:path atomically:YES];
    //UnitySendMessage("Main Camera", "IOSBack", "Temp.jpg");
}

-(void)imagePickerControllerDidCancel:(UIImagePickerController *)picker{
    UnitySendMessage("Main Camera", "IOSBack", "");
    [picker dismissViewControllerAnimated:YES completion:nil];  
}

extern "C"
{
    void IOS_OpenCamera()
    {
        UnityTest *app=[UnityTest shareManager];
        UIViewController *vc = UnityGetGLViewController();
        [vc.view addSubview:app.view];
        
        [app OpenTarget:UIImagePickerControllerSourceTypeCamera];
    }
    
    void IOS_OpenAlbum(int posX, int posY, int width, int height)
    {
       UnityTest *app =[UnityTest shareManager];
        app.x = posX;
        app.y = posY;
        app.w = width;
        app.h = height;
        UIViewController *vc =UnityGetGLViewController();
        [vc.view addSubview:app.view];
        
        [app OpenTarget:UIImagePickerControllerSourceTypePhotoLibrary];
    }

    //根据Unity传过来的坐标(x,y）和 宽高(width,height)显示IOS界面
    void IOS_ShowView(int posX, int posY, int width, int height)
    {
    	UnityTest *app=[UnityTest shareManager];
        UIViewController *vc = UnityGetGLViewController();
        [vc.view addSubview:app.view];

    }

    //关闭并释放这个IOS界面
    void IOS_CloseView()
    {
    	UnityTest *app=[UnityTest shareManager];
        [app deletePlayer];
    }

}
@end
