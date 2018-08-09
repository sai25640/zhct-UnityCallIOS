//
//  UnityTest.h
//  Unity-iPhone
//
//  Created by Leon on 2018/7/27.
//

#ifndef UnityTest_h
#define UnityTest_h

#import <QuartzCore/CADisplayLink.h>

@interface UnityTest : UIViewController<UIImagePickerControllerDelegate,UINavigationControllerDelegate>
+(instancetype)shareManager; //将类设置为单例
@end

#endif /* UnityTest_h */
