import { replace, useNavigate } from 'react-router-dom';
import BackButton from '../../../../shared/ui/back_button/BackButton';
import LexicalComplexity from '../../../../ui/lexical_complexity/LexicalComplexity';
import InfoVideo from '../info_video/InfoVideo';
import styles from '../video_player_header/video_player_header.module.css';

function VideoPlayerHeader({video}){

    const navigation = useNavigate();

    const onNavigateBack = () => {
        navigation(-1, {replace: true})
    };
    return(
        <header className={styles.video_player_header}>
            <BackButton onClick={onNavigateBack}/>
            <InfoVideo video={video}/>
        </header>
    )
}

export default VideoPlayerHeader;