import { useDispatch } from 'react-redux';
import { closeModalViewNote } from '../../../../redux/slices/modalSlice';
import Source from '../../../../ui/source/Source';
import styles from '../noteSource/note_source.module.css';
import { useNavigate } from 'react-router-dom';
import useEmblaCarousel from "embla-carousel-react";
import { useState } from 'react';
import { noteContextApi } from '../../../context/api/noteContext';
import { toast } from 'react-toastify';

function NoteSource({sources, setSourceList, noteId, word}){

    const dispatch = useDispatch();
    const navigation = useNavigate();

    const [isDeleting, setIsDeleting] = useState(false);

    const [emblaRef] = useEmblaCarousel({
        loop:false,
        align:'start',
        slidesToScroll:1
    });

    const getStartTime = (duration) => {
        if(!duration) return 0;

        return (duration.hours || 0) * 3600 + 
                (duration.minutes || 0) * 60 +
                (duration.seconds || 0);
    };

    const onNavigationToPlayer = (video) => {
            dispatch(closeModalViewNote());
            navigation("/video-player", {
                state:{
                    video
                }
            })
    };

    const handleDelete = async(noteId, source) => {
        if(isDeleting) return;

        setIsDeleting(true);

        const response = await noteContextApi.deleteNoteContext(
            noteId,
            source.youtubeVideoId,
            source.durationContext.hours,
            source.durationContext.minutes,
            source.durationContext.seconds
        );

        if(response.success){
            const newSources = sources.filter(item => 
                item.youtubeVideoId !== source.youtubeVideoId || 
                getStartTime(item.durationContext) !== getStartTime(source.durationContext)
            );

            setSourceList(newSources);
            toast.success('Контекст успешно удален');
        }
        else{
            toast.error(response?.error);
        }

        setIsDeleting(false);
    };    
    
    const hasSources = sources.length > 0;
    if(!hasSources) return null;

    return(
        <div className={styles.source_block}>
            <p style={{margin: 0, color: '#6c5a51'}}>Источники ({sources.length})</p>

            <div className={styles.viewport} ref={emblaRef}>
                <div className={styles.container}>
                    {sources.map((item, index) => {
                        const video = {
                            id: item.youtubeVideoId,
                            youtubeId: item.youtubeId,
                            titleVideo: item.titleVideo,
                            startTime: getStartTime(item.durationContext)
                        }

                        return(
                            <div className={styles.slide} key={item.id ?? index}>
                                <Source
                                    youtubeVideoTitle={item.youtubeVideoTitle}
                                    context={item.context}
                                    duration={item.durationContext}
                                    word={word}
                                    onNavigateToPlayer={() => onNavigationToPlayer(video)}
                                    onDelete={() => handleDelete(noteId, item)}
                                />
                            </div>
                        )
                    })}
                </div>
            </div>
        </div>
    )
}

export default NoteSource;